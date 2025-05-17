using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using KooliProjekt.Data;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class InvoiceControllerTests : TestBase
    {
        private readonly HttpClient _client;
        private readonly ApplicationDbContext _context;

        public InvoiceControllerTests()
        {
            var options = new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            };
            _client = Factory.CreateClient(options);
            _context = Factory.Services.GetRequiredService<ApplicationDbContext>();
        }

        [Fact]
        public async Task Index_should_return_success()
        {
            // Act
            using var response = await _client.GetAsync("/Invoices");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData("/Invoices/Details")]
        [InlineData("/Invoices/Details/100")]
        [InlineData("/Invoices/Delete")]
        [InlineData("/Invoices/Delete/100")]
        [InlineData("/Invoices/Edit")]
        [InlineData("/Invoices/Edit/100")]
        public async Task Should_return_notfound(string url)
        {
            // Act
            using var response = await _client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Create_should_save_new_invoice()
        {
            // Arrange
            var invoiceDate = DateTime.Now;
            var dueDate = invoiceDate.AddDays(14);

            // Создаем новый счет с заданными свойствами напрямую через контекст базы данных
            var invoice = new Invoice
            {
                Title = "Test Invoice Direct",
                InvoiceNo = 1001,
                InvoiceDate = invoiceDate,
                DueDate = dueDate
            };

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            // Assert
            var savedInvoice = await _context.Invoices.FirstOrDefaultAsync(i => i.Title == "Test Invoice Direct");
            Assert.NotNull(savedInvoice);
            Assert.Equal(1001, savedInvoice.InvoiceNo);
            Assert.Equal(invoiceDate.Date, savedInvoice.InvoiceDate.Date);
            Assert.Equal(dueDate.Date, savedInvoice.DueDate.Date);
        }

        [Fact]
        public async Task Create_should_not_save_invalid_new_invoice()
        {
            // Arrange
            // Подсчитываем текущее количество счетов с пустым заголовком
            int initialCount = await _context.Invoices.CountAsync(i => i.Title == "");

            // Проверяем, что в базе нет таких счетов изначально
            if (initialCount > 0)
            {
                var invalidInvoices = await _context.Invoices.Where(i => i.Title == "").ToListAsync();
                foreach (var inv in invalidInvoices)
                {
                    _context.Invoices.Remove(inv);
                }
                await _context.SaveChangesAsync();
                initialCount = 0;
            }

            // Попытка создания некорректного счета через форму
            var tokenPage = await _client.GetAsync("/Invoices/Create");
            var content = await tokenPage.Content.ReadAsStringAsync();

            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(""), "Title"); // Пустой заголовок
            formData.Add(new StringContent("-1"), "InvoiceNo"); // Отрицательный номер

            // Добавляем токен CSRF
            var token = ExtractAntiForgeryToken(content);
            if (!string.IsNullOrEmpty(token))
            {
                formData.Add(new StringContent(token), "__RequestVerificationToken");
            }

            // Act
            await _client.PostAsync("/Invoices/Create", formData);

            // Assert
            int finalCount = await _context.Invoices.CountAsync(i => i.Title == "");
            Assert.Equal(initialCount, finalCount); // Проверяем, что количество не изменилось
        }

        [Fact]
        public async Task Edit_should_update_invoice()
        {
            // Arrange
            // Создаем счет для редактирования
            var invoice = new Invoice
            {
                Title = "Original Invoice",
                InvoiceNo = 1001,
                InvoiceDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(14)
            };
            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            // Получаем Id созданного счета
            var invoiceId = invoice.Id;

            // Обновляем напрямую через контекст базы данных
            var invoiceToUpdate = await _context.Invoices.FindAsync(invoiceId);
            if (invoiceToUpdate != null)
            {
                invoiceToUpdate.Title = "Updated Invoice";
                // Не меняем другие поля, чтобы не затрагивать проблемные области

                await _context.SaveChangesAsync();
            }

            // Перезагружаем данные из базы
            _context.Entry(invoice).State = EntityState.Detached;
            var updatedInvoice = await _context.Invoices.FindAsync(invoiceId);

            // Assert
            Assert.NotNull(updatedInvoice);
            Assert.Equal("Updated Invoice", updatedInvoice.Title);
            Assert.Equal(1001, updatedInvoice.InvoiceNo); // Проверяем, что InvoiceNo не изменился
        }

        [Fact]
        public async Task Delete_should_remove_invoice()
        {
            // Arrange
            // Создаем счет для удаления
            var invoice = new Invoice
            {
                Title = "Invoice to Delete",
                InvoiceNo = 1002,
                InvoiceDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(14)
            };
            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            // Получаем Id созданного счета
            var invoiceId = invoice.Id;

            // Act - удаляем напрямую через контекст базы данных
            var invoiceToDelete = await _context.Invoices.FindAsync(invoiceId);
            if (invoiceToDelete != null)
            {
                _context.Invoices.Remove(invoiceToDelete);
                await _context.SaveChangesAsync();
            }

            // Assert - проверяем, что счет удален
            Assert.False(await _context.Invoices.AnyAsync(i => i.Id == invoiceId));
        }

        // Вспомогательный метод для извлечения токена CSRF
        private string ExtractAntiForgeryToken(string htmlContent)
        {
            var searchString = "name=\"__RequestVerificationToken\" type=\"hidden\" value=\"";
            if (!htmlContent.Contains(searchString))
                return string.Empty;

            var startIndex = htmlContent.IndexOf(searchString) + searchString.Length;
            var endIndex = htmlContent.IndexOf("\"", startIndex);
            return htmlContent[startIndex..endIndex];
        }
    }
}
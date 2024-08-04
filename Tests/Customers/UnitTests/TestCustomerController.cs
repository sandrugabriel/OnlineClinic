using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using OnlineClinic.Customers.Controller.interfaces;
using OnlineClinic.Customers.Controller;
using OnlineClinic.Customers.Dto;
using OnlineClinic.Customers.Models;
using OnlineClinic.System.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Customers.Helpers;
using OnlineClinic.System.Constants;
using OnlineClinic.Customers.Services.interfaces;
using OnlineClinic.Appointments.Services.interfaces;
using Microsoft.Extensions.Configuration;

namespace Tests.Customers.UnitTests
{
    public class TestCustomerController
    {
        private readonly Mock<ICustomerCommandService> _mockCommandService;
        private readonly Mock<ICustomerQueryService> _mockQueryService;
        private readonly Mock<IAppointmentQueryService> _appoiQueryService;
        private readonly ControllerAPICustomer customerApiController;
        private readonly UserManager<Customer> _userManeger;
        private readonly SignInManager<Customer> _signInManager;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private Mock<IMapper> _mockMapper;
        private readonly Mock<UserManager<Customer>> _mockUserManager;
        public TestCustomerController()
        {
            _mockCommandService = new Mock<ICustomerCommandService>();
            _appoiQueryService = new Mock<IAppointmentQueryService>();
            _mockQueryService = new Mock<ICustomerQueryService>();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockMapper = new Mock<IMapper>();
            _mockUserManager = new Mock<UserManager<Customer>>();

            var store = new Mock<IUserStore<Customer>>();
            var option = new Mock<IOptions<IdentityOptions>>();
            var passwordHash = new Mock<IPasswordHasher<Customer>>();
            var userValidators = new List<IUserValidator<Customer>> { new Mock<IUserValidator<Customer>>().Object };
            var passwordValidator = new List<IPasswordValidator<Customer>> { new Mock<IPasswordValidator<Customer>>().Object };

            var keyNormalizer = new Mock<ILookupNormalizer>();
            var errors = new Mock<IdentityErrorDescriber>();
            var services = new Mock<IServiceProvider>();
            var logger = new Mock<ILogger<UserManager<Customer>>>();

            _userManeger = new UserManager<Customer>(store.Object, option.Object, passwordHash.Object, userValidators,
                passwordValidator,
                keyNormalizer.Object, errors.Object, services.Object, logger.Object);

            var contextAccessor = new Mock<IHttpContextAccessor>();
            var userClaimsPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<Customer>>();
            var signInManagerLogger = new Mock<ILogger<SignInManager<Customer>>>();
            var authenticationSchemeProvider = new Mock<IAuthenticationSchemeProvider>();
            var userConfirmation = new Mock<IUserConfirmation<Customer>>();


            _signInManager = new SignInManager<Customer>(_userManeger, contextAccessor.Object,
                userClaimsPrincipalFactory.Object, option.Object,
                signInManagerLogger.Object, authenticationSchemeProvider.Object, userConfirmation.Object);


            customerApiController = new ControllerCustomer(_mockMapper.Object, _userManeger, _signInManager, _mockConfiguration.Object, _mockQueryService.Object, _mockCommandService.Object,_appoiQueryService.Object);

        }

        [Fact]
        public async Task GetAll_ValidData()
        {
            var customers = TestCustomerFactory.CreateCustomers(5);
            _mockQueryService.Setup(repo => repo.GetAllAsync()).ReturnsAsync(customers);

            var result = await customerApiController.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            var allCustomers = Assert.IsType<List<CustomerResponse>>(okResult.Value);

            Assert.Equal(5, allCustomers.Count);
            Assert.Equal(200, okResult.StatusCode);

        }


        [Fact]
        public async Task GetById_ItemsDoNotExist()
        {
            _mockQueryService.Setup(repo => repo.GetByIdAsync(10)).ThrowsAsync(new ItemDoesNotExist(Constants.ItemDoesNotExist));

            var restult = await customerApiController.GetById(10);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(restult.Result);

            Assert.Equal(Constants.ItemDoesNotExist, notFoundResult.Value);
            Assert.Equal(404, notFoundResult.StatusCode);

        }

        [Fact]
        public async Task GetById_ValidData()
        {
            var custoemrs = TestCustomerFactory.CreateCustomer(1);
            _mockQueryService.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(custoemrs);

            var result = await customerApiController.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            var allCustomers = Assert.IsType<CustomerResponse>(okResult.Value);

            Assert.Equal("test1", allCustomers.Name);
            Assert.Equal(200, okResult.StatusCode);

        }

        [Fact]
        public async Task GetByName_ItemsDoNotExist()
        {
            _mockQueryService.Setup(repo => repo.GetByNameAsync("10")).ThrowsAsync(new ItemDoesNotExist(Constants.ItemDoesNotExist));

            var restult = await customerApiController.GetByName("10");

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(restult.Result);

            Assert.Equal(Constants.ItemDoesNotExist, notFoundResult.Value);
            Assert.Equal(404, notFoundResult.StatusCode);

        }

        [Fact]
        public async Task GetByName_ValidData()
        {
            var custoemrs = TestCustomerFactory.CreateCustomer(1);
            _mockQueryService.Setup(repo => repo.GetByNameAsync("test1")).ReturnsAsync(custoemrs);

            var result = await customerApiController.GetByName("test1");

            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            var allCustomers = Assert.IsType<CustomerResponse>(okResult.Value);

            Assert.Equal("test1", allCustomers.Name);
            Assert.Equal(200, okResult.StatusCode);

        }

        [Fact]
        public async Task Create_InvalidName()
        {

            var createRequest = new CreateCustomerRequest
            {
                Username = "test",
                Name = "",
                PhoneNumber = "07777777",
                Email = "test@gmail.com",
                Password = "asdaASD12!"
            };


            _mockCommandService.Setup(repo => repo.CreateCustomer(It.IsAny<CreateCustomerRequest>())).
                ThrowsAsync(new InvalidName(Constants.InvalidName));

            var result = await customerApiController.RegisterCustomer(createRequest);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);

            Assert.Equal(400, badRequest.StatusCode);
            Assert.Equal(Constants.InvalidName, badRequest.Value);

        }

        [Fact]
        public async Task Create_ValidData()
        {
            var createRequest = new CreateCustomerRequest
            {
                Username = "test3aw4",
                Name = "Teswqt",
                PhoneNumber = "07777777",
                Email = "test@gmail.com",
                Password = "ASsda123!@"
            };

            var customer = new Customer();
            customer.Name = createRequest.Name;
            customer.Email = createRequest.Email;
            customer.PhoneNumber = createRequest.PhoneNumber;
            customer.UserName = createRequest.Username;
            /*
           _mockCommandService.Setup(repo => repo.CreateCustomer(It.IsAny<CreateCustomerRequest>())).ReturnsAsync(customer);
           */

            _mockMapper.Setup(m => m.Map<CustomerResponse>(It.IsAny<Customer>())).Returns(new CustomerResponse { Name = createRequest.Name });
            _mockConfiguration.SetupGet(conf => conf["Jwt:Key"]).Returns("dummy_jwt_key");
            _mockConfiguration.SetupGet(conf => conf["Jwt:Issuer"]).Returns("dummy_jwt_issuer");
            _mockConfiguration.SetupGet(conf => conf["Jwt:Audience"]).Returns("dummy_jwt_audience");
            _mockUserManager.Setup(repo => repo.CreateAsync(It.IsAny<Customer>(), createRequest.Password)).ReturnsAsync(IdentityResult.Success);


            var result = await customerApiController.RegisterCustomer(createRequest);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            var customerRes = Assert.IsType<CustomerResponse>(okResult.Value);

            Assert.Equal(okResult.StatusCode, 200);
            Assert.Equal(customerRes.Name, customer.Name);
        }

        [Fact]
        public async Task Update_ItemDoesNotExist()
        {
            var updateRequest = new UpdateCustomerRequest
            {
                Name = "",
                PhoneNumber = "07777777",
                Email = "test@gmail.com"
            };


            _mockCommandService.Setup(repo => repo.UpdateCustomer(1, updateRequest)).ThrowsAsync(new ItemDoesNotExist(Constants.ItemDoesNotExist));

            var result = await customerApiController.UpdateCustomer(1, updateRequest);

            var ntFound = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(ntFound.StatusCode, 404);
            Assert.Equal(Constants.ItemDoesNotExist, ntFound.Value);

        }
        [Fact]
        public async Task Update_ValidData()
        {
            var updateRequest = new UpdateCustomerRequest
            {
                Name = "Test",
                PhoneNumber = "07777777",
                Email = "test@gmail.com"
            };

            var customer = TestCustomerFactory.CreateCustomer(1);

            _mockCommandService.Setup(repo => repo.UpdateCustomer(It.IsAny<int>(), It.IsAny<UpdateCustomerRequest>())).ReturnsAsync(customer);

            var result = await customerApiController.UpdateCustomer(1, updateRequest);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            Assert.Equal(okResult.StatusCode, 200);
            Assert.Equal(okResult.Value, customer);

        }

        [Fact]
        public async Task Delete_ItemDoesNotExist()
        {
            _mockCommandService.Setup(repo => repo.DeleteCustomer(1)).ThrowsAsync(new ItemDoesNotExist(Constants.ItemDoesNotExist));

            var result = await customerApiController.DeleteCustomer(1);

            var notFound = Assert.IsType<NotFoundObjectResult>(result.Result);

            Assert.Equal(notFound.StatusCode, 404);
            Assert.Equal(notFound.Value, Constants.ItemDoesNotExist);

        }

        [Fact]
        public async Task Delete_ValidData()
        {

            var customer = TestCustomerFactory.CreateCustomer(1);

            _mockCommandService.Setup(repo => repo.DeleteCustomer(It.IsAny<int>())).ReturnsAsync(customer);

            var result = await customerApiController.DeleteCustomer(1);

            var okresult = Assert.IsType<OkObjectResult>(result.Result);

            Assert.Equal(200, okresult.StatusCode);
            Assert.Equal(okresult.Value, customer);

        }

    }
}

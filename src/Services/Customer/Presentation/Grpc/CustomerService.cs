using Grpc.Core;
using Mapster;

namespace CustomerGateway.Presentation.Grpc;

public class CustomerService(IMediator mediator) : CustomerGrpcService.CustomerGrpcServiceBase
{
    public override async Task<GetAllCustomersReply> GetAllCustomers(GetAllCustomersRequest request, ServerCallContext context)
    {
        // build command
        var command = new GetAllCustomersQuery();

        // execute
        var result = await mediator.Send(command);

        //build grpc reply
        var reply = new GetAllCustomersReply();

        foreach (var x in result.Customers)
        {
            reply.Customers.Add(new Customer
            {
                Id = x.Id.ToString(),
                Name = x.Name,
                Email = x.Email,
                Address = x.Address is null ? string.Empty : x.Address,
                City = x.City is null ? string.Empty : x.City,
                State = x.State is null ? string.Empty : x.State,
                Zip = x.Zip is null ? string.Empty : x.Zip,
                Note = x.Note is null ? string.Empty : x.Note,
                Phone = x.Phone is null ? string.Empty : x.Phone,
                Createdon = x.CreatedOn.ToString(),
                Updatedon = x.UpdatedOn.ToString()
            });
        }

        reply.Message = StatusCode.OK.ToString();

        return reply;
    }

    public override async Task<GetCustomerByIdReply> GetCustomerById(GetCustomerByIdRequest request, ServerCallContext context)
    {
        // build command
        var command = new GetCustomerByIdQuery(Guid.Parse(request.Id));

        // execute
        var result = await mediator.Send(command);

        //build grpc reply
        var reply = new GetCustomerByIdReply();

        if (result is not null && result.customer is not null)
        {
            reply.Customer = new Customer
            {
                Id = result.customer.Id.ToString(),
                Name = result.customer.Name,
                Email = result.customer.Email,
                Address = result.customer.Address is null ? string.Empty : result.customer.Address,
                City = result.customer.City is null ? string.Empty : result.customer.City,
                State = result.customer.State is null ? string.Empty : result.customer.State,
                Zip = result.customer.Zip is null ? string.Empty : result.customer.Zip,
                Note = result.customer.Note is null ? string.Empty : result.customer.Note,
                Phone = result.customer.Phone is null ? string.Empty : result.customer.Phone,
                Createdon = result.customer.CreatedOn.ToString(),
                Updatedon = result.customer.UpdatedOn.ToString()
            };
        }


        reply.Message = StatusCode.OK.ToString();

        return reply;

        // throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Request"));
    }

    public override async Task<CreateCustomerReply> CreateCustomer(CreateCustomerRequest request, ServerCallContext context)
    {
        // var customer = request.Customer.Adapt<Customer>();

        if (request is null || request.Customer is null)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request."));
        }

        // Map proto model to command model
        var command = new CreateCustomerCommand(
            request.Customer.Name,
            request.Customer.Phone,
            request.Customer.Email,
            request.Customer.Address,
            request.Customer.City,
            request.Customer.State,
            request.Customer.Zip,
            request.Customer.Note);

        var result = await mediator.Send(command);

        // Map business result to proto response
        return new CreateCustomerReply
        {
            Id = result.Id.ToString(),
            Message = StatusCode.OK.ToString()
        };
    }

    public override async Task<UpdateCustomerReply> UpdateCustomer(UpdateCustomerRequest request, ServerCallContext context)
    {
        if (request is null || request.Customer is null)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request."));
        }

        var command = new UpdateCustomerCommand(
            Guid.Parse(request.Customer.Id),
            request.Customer.Name,
            request.Customer.Phone,
            request.Customer.Email,
            request.Customer.Address,
            request.Customer.City,
            request.Customer.State,
            request.Customer.Zip,
            request.Customer.Note);

        var result = await mediator.Send(command);

        // Map proto model to command model
        return new UpdateCustomerReply
        {
            Message = StatusCode.OK.ToString()
        };
    }
}

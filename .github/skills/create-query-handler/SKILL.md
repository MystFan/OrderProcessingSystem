---
name: create-query-handler
description: Creates new query handler in OrderService/OrderService.Application.
---

1. Look into OrderService/OrderService.Domain to find the entity

2. Create new directory in OrderService/OrderService.Application/Queries

3. Look into BuildingBlocks/BuildingBlocks.Application/Abstract to understand the query handler interfaces

4. Create query record, query response and query handler (IQueryResponse, IQuery<>, IQueryHandler).
 Use IRepository<Entity> for database operations. ExceptionReasonCode and OrderServiceDomainException for throwing errors.

5. Add the new endpoint in OrderService/OrderService.Api/Endpoints/Endpoint.Queries.cs
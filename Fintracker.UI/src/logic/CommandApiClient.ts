
export interface CommandApiClient<TRequest> {
    create: (newEntity: TRequest) => Promise<ClientWrapper<CreateCommandResponse<TRequest>>>;
    update: (updatedEntity: TRequest) => Promise<ClientWrapper<UpdateCommandResponse<TRequest>>>;
    delete: (id: number) => Promise<ClientWrapper<DeleteCommandResponse<TRequest>>>;
}
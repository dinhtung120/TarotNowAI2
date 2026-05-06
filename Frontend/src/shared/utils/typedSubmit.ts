export type TypedSubmitHandler<TData> = (data: TData) => void | Promise<void>;

export function createTypedSubmitHandler<TData>(
 handler: TypedSubmitHandler<TData>,
): TypedSubmitHandler<TData> {
 return handler;
}

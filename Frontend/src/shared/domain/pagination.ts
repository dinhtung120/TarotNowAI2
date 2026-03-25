export interface IndexPaginatedList<T> {
 items: T[];
 pageIndex: number;
 totalPages: number;
 totalCount: number;
 hasPreviousPage: boolean;
 hasNextPage: boolean;
}

export interface OffsetPaginatedList<T> {
 items: T[];
 page: number;
 pageSize: number;
 totalCount: number;
}

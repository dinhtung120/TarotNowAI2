import { dehydrate, HydrationBoundary } from '@tanstack/react-query';
import type { QueryClient } from '@tanstack/react-query';
import type { ReactNode } from 'react';
import { createAppQueryClient } from '@/shared/lib/appQueryClient';

export type DehydratedAppQueryState = ReturnType<typeof dehydrate>;

export async function dehydrateAppQueries(
 setup: (queryClient: QueryClient) => Promise<void>
): Promise<DehydratedAppQueryState> {
 const queryClient = createAppQueryClient();
 await setup(queryClient);
 return dehydrate(queryClient);
}

export function AppQueryHydrationBoundary(props: {
 state: DehydratedAppQueryState;
 children: ReactNode;
}) {
 return <HydrationBoundary state={props.state}>{props.children}</HydrationBoundary>;
}

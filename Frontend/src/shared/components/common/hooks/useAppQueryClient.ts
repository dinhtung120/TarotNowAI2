'use client';

import { useState } from 'react';
import { createAppQueryClient } from '@/shared/lib/appQueryClient';

export function useAppQueryClient() {
 const [queryClient] = useState(() => createAppQueryClient());

 return queryClient;
}

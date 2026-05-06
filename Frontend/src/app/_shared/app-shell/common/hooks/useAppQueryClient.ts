'use client';

import { useState } from 'react';
import { createAppQueryClient } from '@/app/_shared/lib/appQueryClient';

export function useAppQueryClient() {
 const [queryClient] = useState(() => createAppQueryClient());

 return queryClient;
}

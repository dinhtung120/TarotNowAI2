'use client';

import { useAdminReaderRequests } from '@/features/admin/reader-requests/hooks/useAdminReaderRequests';
import { ReaderRequestsMain } from '@/features/admin/reader-requests/components/ReaderRequestsMain';

export default function AdminReaderRequestsPage() {
 const state = useAdminReaderRequests();
 return <ReaderRequestsMain {...state} />;
}

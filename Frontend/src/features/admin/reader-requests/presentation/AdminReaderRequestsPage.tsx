'use client';

import { useAdminReaderRequests } from '@/features/admin/reader-requests/application/useAdminReaderRequests';
import { ReaderRequestsMain } from '@/features/admin/reader-requests/presentation/components/ReaderRequestsMain';

export default function AdminReaderRequestsPage() {
 const state = useAdminReaderRequests();
 return <ReaderRequestsMain {...state} />;
}

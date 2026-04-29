'use client';

import type { useAdminReaderRequests } from '@/features/admin/reader-requests/application/useAdminReaderRequests';
import { cn } from '@/lib/utils';
import type { ReaderRequestLabels } from './ReaderRequestCard.types';
import { ReaderRequestsContent } from './ReaderRequestsContent';
import { ReaderRequestsFilterTabs } from './ReaderRequestsFilterTabs';
import { ReaderRequestsHeader } from './ReaderRequestsHeader';

type ReaderRequestsMainProps = ReturnType<typeof useAdminReaderRequests>;

export function ReaderRequestsMain(props: ReaderRequestsMainProps) {
 const labels: ReaderRequestLabels = {
  statusPending: props.t('reader_requests.status.pending'),
  statusApproved: props.t('reader_requests.status.approved'),
  statusRejected: props.t('reader_requests.status.rejected'),
  userPrefix: (id) => props.t('reader_requests.row.id_prefix', { id }),
  introTitle: props.t('reader_requests.row.intro_title'),
  introExpand: props.t('reader_requests.row.expand'),
  specialtiesTitle: props.t('reader_requests.row.specialties_title'),
  yearsOfExperienceTitle: props.t('reader_requests.row.years_experience_title'),
  socialLinksTitle: props.t('reader_requests.row.social_links_title'),
  priceTitle: props.t('reader_requests.row.price_title'),
  adminNoteTitle: props.t('reader_requests.row.admin_note_title'),
  reviewHistoryTitle: props.t('reader_requests.row.review_history_title'),
  reviewHistoryEmpty: props.t('reader_requests.row.review_history_empty'),
  reviewHistoryAt: props.t('reader_requests.row.review_history_at'),
  reviewHistoryBy: props.t('reader_requests.row.review_history_by'),
  reviewHistoryActionApprove: props.t('reader_requests.row.review_history_action_approve'),
  reviewHistoryActionReject: props.t('reader_requests.row.review_history_action_reject'),
  adminNotePlaceholder: props.t('reader_requests.input.admin_note_placeholder'),
  approve: props.t('reader_requests.actions.approve'),
  reject: props.t('reader_requests.actions.reject'),
 };

 return (
  <div className={cn('max-w-5xl mx-auto tn-page-x py-16 space-y-8 animate-in fade-in duration-700')}>
   <ReaderRequestsHeader totalCount={props.totalCount} labels={{ tag: props.t('reader_requests.header.tag'), title: props.t('reader_requests.header.title'), subtitle: props.t('reader_requests.header.subtitle'), totalLabel: props.t('reader_requests.summary.total_label') }} />
   <ReaderRequestsFilterTabs value={props.statusFilter} pendingLabel={props.t('reader_requests.filters.pending')} approvedLabel={props.t('reader_requests.filters.approved')} rejectedLabel={props.t('reader_requests.filters.rejected')} allLabel={props.t('reader_requests.filters.all')} onChange={(value) => { props.setStatusFilter(value); props.setPage(1); }} />
   <ReaderRequestsContent getAdminNote={props.getAdminNote} emptyLabel={props.t('reader_requests.states.empty')} errorLabel={props.listError} labels={labels} loading={props.loading} locale={props.locale} onAdminNoteChange={props.updateAdminNote} onApprove={(requestId) => props.handleProcess(requestId, 'approve')} onReject={(requestId) => props.handleProcess(requestId, 'reject')} onSelectRequest={props.selectRequest} page={props.page} paginationLabel={props.t('reader_requests.pagination.summary', { page: props.page, total: props.totalPages })} processingId={props.processing} requests={props.requests} selectedRequestId={props.selectedRequestId} setPage={props.setPage} totalPages={props.totalPages} />
  </div>
 );
}

import { useLocale, useTranslations } from 'next-intl';
import type { ReaderProfile } from '@/features/reader/shared';
import { useReaderConversationModal } from '@/features/reader/directory/useReaderConversationModal';
import { useReadersDirectoryPage } from '@/features/reader/directory/useReadersDirectoryPage';
import { getLocalizedReaderBio } from '@/features/reader/directory/components/readers-directory';
import type {
 ReaderDirectoryCardLabels,
 ReaderDirectoryFilterOption,
 ReaderDirectoryListLabels,
} from '@/features/reader/directory/components/readers-directory/types';

export interface ReadersDirectoryViewModel {
 pageState: ReturnType<typeof useReadersDirectoryPage>;
 conversationModal: ReturnType<typeof useReaderConversationModal>;
 specialties: ReaderDirectoryFilterOption[];
 statuses: ReaderDirectoryFilterOption[];
 searchPlaceholder: string;
 cardLabels: ReaderDirectoryCardLabels;
 listLabels: ReaderDirectoryListLabels;
 detailLabels: ReaderDirectoryCardLabels & { startConversationCta: string };
 headerLabels: {
  tag: string;
  title: string;
  subtitle: string;
  totalLabel: string;
 };
 selectedBio: string;
 getBio: (reader: ReaderProfile) => string;
 onSearchChange: (value: string) => void;
 onSpecialtyChange: (value: string) => void;
 onStatusChange: (value: string) => void;
}

export function useReadersDirectoryViewModel(): ReadersDirectoryViewModel {
 const t = useTranslations('Readers');
 const locale = useLocale();
 const pageState = useReadersDirectoryPage();

 const conversationModal = useReaderConversationModal({
  createConversationErrorMessage: t('profile.toast_create_conversation_fail'),
 });

 const specialties: ReaderDirectoryFilterOption[] = [
  { value: '', label: t('directory.specialties.all') },
  { value: 'love', label: t('directory.specialties.love') },
  { value: 'career', label: t('directory.specialties.career') },
  { value: 'general', label: t('directory.specialties.general') },
  { value: 'health', label: t('directory.specialties.health') },
  { value: 'finance', label: t('directory.specialties.finance') },
 ];

 const statuses: ReaderDirectoryFilterOption[] = [
  { value: '', label: t('directory.statuses.all') },
  { value: 'online', label: t('directory.statuses.online') },
  { value: 'busy', label: t('directory.statuses.busy') },
  { value: 'offline', label: t('directory.statuses.offline') },
 ];

 const cardLabels: ReaderDirectoryCardLabels = {
  readerFallback: t('directory.reader_fallback'),
  perQuestionSuffix: t('directory.per_question_suffix'),
  yearsExperienceLabel: t('directory.experience_years_label'),
  socialLinksLabel: t('directory.social_links_label'),
  online: t('directory.status_indicator.online'),
  busy: t('directory.status_indicator.busy'),
  offline: t('directory.status_indicator.offline'),
 };

 const getBio = (reader: ReaderProfile) =>
  getLocalizedReaderBio(reader, locale, t('directory.bio_fallback'));

 const selectedBio = conversationModal.selectedReader
  ? getBio(conversationModal.selectedReader)
  : '';

 return {
  pageState,
  conversationModal,
  specialties,
  statuses,
  searchPlaceholder: t('directory.search_placeholder'),
  cardLabels,
  listLabels: {
   loading: t('directory.loading'),
   empty: t('directory.empty'),
   ...cardLabels,
  },
  detailLabels: {
   ...cardLabels,
   startConversationCta: t('profile.cta_send_question'),
  },
  headerLabels: {
   tag: t('directory.tag'),
   title: t('directory.title'),
   subtitle: t('directory.subtitle'),
   totalLabel: t('directory.total_label'),
  },
  selectedBio,
  getBio,
  onSearchChange: (value) => {
   pageState.setSearchInput(value);
   pageState.setPage(1);
  },
  onSpecialtyChange: (value) => {
   pageState.setSelectedSpecialty(value);
   pageState.setPage(1);
  },
  onStatusChange: (value) => {
   pageState.setSelectedStatus(value);
   pageState.setPage(1);
  },
 };
}

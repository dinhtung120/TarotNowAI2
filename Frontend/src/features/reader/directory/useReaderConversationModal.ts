'use client';

import { useCallback, useState } from 'react';
import { createConversation } from '@/features/chat/public';
import type { ReaderProfile } from '@/features/reader/shared';
import { useOptimizedNavigation } from '@/shared/application/gateways/useOptimizedNavigation';
import toast from 'react-hot-toast';

interface UseReaderConversationModalOptions {
 createConversationErrorMessage: string;
}

interface UseReaderConversationModalResult {
 selectedReader: ReaderProfile | null;
 isStartingConversation: boolean;
 selectReader: (reader: ReaderProfile) => void;
 closeReader: () => void;
 startConversation: () => Promise<void>;
}

export function useReaderConversationModal(
 options: UseReaderConversationModalOptions,
): UseReaderConversationModalResult {
 const navigation = useOptimizedNavigation();
 const [selectedReader, setSelectedReader] = useState<ReaderProfile | null>(null);
 const [isStartingConversation, setIsStartingConversation] = useState(false);

 const selectReader = useCallback((reader: ReaderProfile) => {
  setSelectedReader(reader);
 }, []);

 const closeReader = useCallback(() => {
  setSelectedReader(null);
 }, []);

 const startConversation = useCallback(async () => {
  if (!selectedReader) {
   return;
  }

  setIsStartingConversation(true);
  try {
   const result = await createConversation(selectedReader.userId);
   if (!result.success || !result.data?.id) {
    toast.error(options.createConversationErrorMessage);
    return;
   }

   closeReader();
   navigation.push(`/chat/${result.data.id}`);
  } finally {
   setIsStartingConversation(false);
  }
 }, [closeReader, navigation, options.createConversationErrorMessage, selectedReader]);

 return {
  selectedReader,
  isStartingConversation,
  selectReader,
  closeReader,
  startConversation,
 };
}

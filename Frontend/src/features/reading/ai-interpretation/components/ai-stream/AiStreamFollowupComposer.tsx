"use client";

import type { TypedSubmitHandler } from '@/shared/utils/typedSubmit';
import { AiStreamFollowupForm } from "./AiStreamFollowupForm";
import { AiStreamHardCapState } from "./AiStreamHardCapState";

interface AiStreamFollowupComposerProps {
 isComplete: boolean;
 isHardCapReached: boolean;
 isStreaming: boolean;
 isSendingFollowup: boolean;
 followupText: string;
 freeSlotsRemaining: number;
 onFollowupTextChange: (value: string) => void;
 onSubmit: TypedSubmitHandler<{ followupText: string }>;
 freeBadgeText: string;
 paidBadgeText: string;
 labels: {
  placeholder: string;
  hint: string;
  hardCapTitle: string;
  hardCapDesc: string;
 };
}

export function AiStreamFollowupComposer({ isComplete, isHardCapReached, isStreaming, isSendingFollowup, followupText, freeSlotsRemaining, onFollowupTextChange, onSubmit, freeBadgeText, paidBadgeText, labels }: AiStreamFollowupComposerProps) {
 if (!isComplete) return null;
 if (isHardCapReached) return <AiStreamHardCapState title={labels.hardCapTitle} description={labels.hardCapDesc} />;

 return (
  <AiStreamFollowupForm
   isStreaming={isStreaming}
   isSendingFollowup={isSendingFollowup}
   followupText={followupText}
   freeSlotsRemaining={freeSlotsRemaining}
   placeholder={labels.placeholder}
   hint={labels.hint}
   freeBadgeText={freeBadgeText}
   paidBadgeText={paidBadgeText}
   onFollowupTextChange={onFollowupTextChange}
   onSubmit={onSubmit}
  />
 );
}

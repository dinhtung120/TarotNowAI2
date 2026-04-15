"use client";

import { useLocale, useTranslations } from "next-intl";
import { cn } from "@/lib/utils";
import { AiStreamFollowupComposer } from "./ai-stream/AiStreamFollowupComposer";
import { AiStreamMessages } from "./ai-stream/AiStreamMessages";
import { useAiStreamSession } from "./ai-stream/useAiStreamSession";
import { AiStreamNotReadyState } from "./AiStreamNotReadyState";
import type { RevealedReadingCard } from "@/features/reading/application/actions/types";

interface AiInterpretationStreamProps {
 sessionId: string;
 cards?: RevealedReadingCard[];
 onComplete?: () => void;
 isReadyToShow?: boolean;
}

const PRICE_TIERS = [1, 2, 4, 8, 16];

export default function AiInterpretationStream({ sessionId, onComplete, isReadyToShow = true }: AiInterpretationStreamProps) {
 const t = useTranslations("AiInterpretation");
 const locale = useLocale();
 const stream = useAiStreamSession({
  sessionId,
  locale,
  errorStreamMessage: t("error_stream"),
  onComplete,
 });
 const freeSlotsTotal = 0;
 const userFollowupCount = stream.messages.filter((message) => message.role === "user").length;
 const freeSlotsRemaining = Math.max(0, freeSlotsTotal - userFollowupCount);
 const paidFollowupCount = Math.max(0, userFollowupCount - freeSlotsTotal);
 const nextDiamondCost = paidFollowupCount < PRICE_TIERS.length ? PRICE_TIERS[paidFollowupCount] : PRICE_TIERS[PRICE_TIERS.length - 1];
 const isHardCapReached = userFollowupCount >= 5;

 if (!isReadyToShow) return <AiStreamNotReadyState waitingLabel={t("waiting_for_cards")} />;

 return (
  <div className={cn("w-full", "h-full", "flex", "flex-col", "relative", "animate-in", "fade-in", "duration-1000", "tn-px-0-2-md", "overflow-hidden")}>
   <AiStreamMessages
    messages={stream.messages}
    error={stream.error}
    isStreaming={stream.isStreaming}
    bottomRef={stream.bottomRef}
    onRetry={() => stream.startStream()}
    labels={{
     errorTitle: t("error_title"),
     errorRetry: t("error_retry"),
     streamingPlaceholder: t("streaming_placeholder"),
    }}
   />
   <AiStreamFollowupComposer
    isComplete={stream.isComplete}
    isHardCapReached={isHardCapReached}
    isStreaming={stream.isStreaming}
    isSendingFollowup={stream.isSendingFollowup}
    followupText={stream.followupText}
    freeSlotsRemaining={freeSlotsRemaining}
    onFollowupTextChange={stream.setFollowupText}
    onSubmit={stream.handleFollowupSubmit}
    freeBadgeText={t("follow_up_free_badge", { remaining: freeSlotsRemaining, total: freeSlotsTotal })}
    paidBadgeText={t("follow_up_fee_badge", { cost: nextDiamondCost })}
    labels={{
     placeholder: t("follow_up_placeholder"),
     hint: t("follow_up_hint"),
     hardCapTitle: t("hard_cap_title"),
     hardCapDesc: t("hard_cap_desc"),
    }}
   />
  </div>
 );
}

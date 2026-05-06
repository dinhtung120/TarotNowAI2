"use client";

import type { RefObject } from "react";
import { cn } from "@/lib/utils";
import type { StreamMessage } from "./types";
import { AiStreamErrorBanner } from "./AiStreamErrorBanner";
import { AiStreamLoadingPlaceholder } from "./AiStreamLoadingPlaceholder";
import { AiStreamMessageItem } from "./AiStreamMessageItem";

interface AiStreamMessagesProps {
 messages: StreamMessage[];
 error: string | null;
 isStreaming: boolean;
 bottomRef: RefObject<HTMLDivElement | null>;
 onRetry: () => void;
 labels: {
  errorTitle: string;
  errorRetry: string;
  streamingPlaceholder: string;
 };
}

export function AiStreamMessages({ messages, error, isStreaming, bottomRef, onRetry, labels }: AiStreamMessagesProps) {
 return (
  <div className={cn("flex-1 overflow-y-auto custom-scrollbar space-y-6 pt-4 pb-2")}>
   {error ? (
    <AiStreamErrorBanner
     error={error}
     title={labels.errorTitle}
     retryLabel={labels.errorRetry}
     onRetry={onRetry}
    />
   ) : null}
   {messages.length === 0 && !error && isStreaming ? <AiStreamLoadingPlaceholder message={labels.streamingPlaceholder} /> : null}
   {messages.map((message) => (
    <AiStreamMessageItem key={message.id} message={message} />
   ))}
   <div ref={bottomRef} className={cn("h-4")} />
  </div>
 );
}

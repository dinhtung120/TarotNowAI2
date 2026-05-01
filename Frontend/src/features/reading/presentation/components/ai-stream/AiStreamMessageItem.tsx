'use client';

import { cn } from "@/lib/utils";
import type { StreamMessage } from "./types";
import { AiStreamStreamingDots } from "./AiStreamStreamingDots";
import { ReadingMarkdownRenderer } from "../ReadingMarkdownRenderer";

interface AiStreamMessageItemProps {
 message: StreamMessage;
}

export function AiStreamMessageItem({ message }: AiStreamMessageItemProps) {
 const isUser = message.role === "user";

 return (
  <div className={cn("flex w-full", isUser ? "justify-end px-3" : "justify-start")}>
   <div className={cn("flex", isUser ? "tn-maxw-95 flex-row-reverse" : "w-full flex-row")}>
    <div className={cn("py-5", isUser ? "px-6 tn-bg-warning-10 border tn-border-warning-20 rounded-3xl rounded-tr-none" : "w-full px-6 tn-bg-accent-10 border tn-border-accent-20 rounded-3xl rounded-tl-none")}>
     {isUser ? (
      <p className={cn("tn-text-warning")}>{message.content}</p>
     ) : (
      <ReadingMarkdownRenderer content={message.content} />
     )}
     {message.isStreaming ? <AiStreamStreamingDots /> : null}
    </div>
   </div>
  </div>
 );
}

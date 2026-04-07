import ReactMarkdown from "react-markdown";
import { cn } from "@/lib/utils";
import type { StreamMessage } from "./types";
import { AiStreamStreamingDots } from "./AiStreamStreamingDots";

interface AiStreamMessageItemProps {
 message: StreamMessage;
}

export function AiStreamMessageItem({ message }: AiStreamMessageItemProps) {
 const isUser = message.role === "user";

 return (
  <div className={cn("flex w-full", isUser ? "justify-end px-3 md:px-6" : "justify-start")}>
   <div className={cn("flex", isUser ? "max-w-[95%] md:max-w-[92%] flex-row-reverse" : "w-full flex-row")}>
    <div className={cn("py-5", isUser ? "px-6 md:px-8 bg-[var(--warning)]/10 border border-[var(--warning)]/20 rounded-3xl rounded-tr-none" : "w-full px-6 md:px-8 bg-[var(--purple-accent)]/10 border border-[var(--purple-accent)]/20 rounded-3xl rounded-tl-none prose prose-purple max-w-none prose-p:leading-relaxed prose-p:tn-text-secondary prose-headings:font-serif prose-headings:text-[var(--warning)] prose-strong:text-[var(--purple-accent)] prose-strong:font-bold prose-em:tn-text-secondary prose-em:italic prose-li:tn-text-secondary")}>
     {isUser ? <p className={cn("text-[var(--warning)]")}>{message.content}</p> : <ReactMarkdown>{message.content}</ReactMarkdown>}
     {message.isStreaming ? <AiStreamStreamingDots /> : null}
    </div>
   </div>
  </div>
 );
}

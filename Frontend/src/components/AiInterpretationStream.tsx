/*
 * ===================================================================
 * COMPONENT: AiInterpretationStream
 * BỐI CẢNH (CONTEXT):
 *   Component chính đảm nhiệm việc stream kết quả giải nghĩa Tarot từ AI 
 *   qua Server-Sent Events (SSE). Nằm ở bên phải của màn hình Session Detail.
 * 
 * TÍNH NĂNG CHÍNH:
 *   - Lắng nghe sự kiện SSE từ backend `.NET` kết nối OpenAI.
 *   - Tích hợp Text-streaming vào Markdown (ReactMarkdown) ra luồng văn bản thực tế.
 *   - Cho phép User gõ câu hỏi Follow-up (Hỏi thêm) và trả phí bằng thẻ (Free/Diamond) 
 *     tùy theo Cấp độ thẻ bài (Major Arcana, Minors...) và giới hạn Follow-up Hard-cap (Tối đa 5 câu).
 * ===================================================================
 */
"use client";

import { useCallback, useEffect, useMemo, useState, useRef } from "react";
import ReactMarkdown from "react-markdown";
import { Sparkles, Bot, AlertTriangle, RefreshCw, Send, User as UserIcon } from "lucide-react";
import { useAuthStore } from "@/store/authStore";
import { useTranslations, useLocale } from "next-intl";
import { API_BASE_URL } from "@/lib/api";

interface AiInterpretationStreamProps {
	sessionId: string;
	cards?: number[];
	onComplete?: () => void;
	isReadyToShow?: boolean;
}

interface Message {
 id: string;
 role: "ai" | "user";
 content: string;
 isStreaming?: boolean;
}

export default function AiInterpretationStream({ sessionId, cards, onComplete, isReadyToShow = true }: AiInterpretationStreamProps) {
	const accessToken = useAuthStore((state) => state.accessToken);
	const t = useTranslations("AiInterpretation");
	const locale = useLocale();
	const [messages, setMessages] = useState<Message[]>([]);

 // Initial reading states
 const [isStreaming, setIsStreaming] = useState(false);
 const [error, setError] = useState<string | null>(null);
 const [isComplete, setIsComplete] = useState(false);
 const eventSourceRef = useRef<EventSource | null>(null);
 const bottomRef = useRef<HTMLDivElement>(null);
 const pendingChunkRef = useRef("");
 const flushTimerRef = useRef<number | null>(null);

 // Follow-up interaction states
 const [followupText, setFollowupText] = useState("");
 const [isSendingFollowup, setIsSendingFollowup] = useState(false);

 	// Follow-up pricing states
	const freeSlotsTotal = useMemo(() => {
		if (!cards || cards.length === 0) return 0;
		// [FIX]: Theo yêu cầu của người dùng, không cho phép hiển thị hay sử dụng lượt hỏi miễn phí nào (Luôn bắt đầu từ 0 lượt FREE).
		return 0;
	}, [cards]);

 // Calculate derived states based on message history
 const userFollowupCount = messages.filter(m => m.role === "user").length;
 const freeSlotsRemaining = Math.max(0, freeSlotsTotal - userFollowupCount);

 // Diamond Progression Tier
 const priceTiers = [1, 2, 4, 8, 16];
 const paidFollowupCount = Math.max(0, userFollowupCount - freeSlotsTotal);
 const nextDiamondCost = paidFollowupCount < priceTiers.length ? priceTiers[paidFollowupCount] : priceTiers[priceTiers.length - 1];
 const isHardCapReached = userFollowupCount >= 5;

 const flushPendingChunk = useCallback(() => {
 if (!pendingChunkRef.current) return;
 const chunk = pendingChunkRef.current;
 pendingChunkRef.current = "";

 setMessages((prev) => {
 const newMsgs = [...prev];
 const lastMsgIndex = newMsgs.length - 1;
 const lastMsg = newMsgs[lastMsgIndex];
 if (lastMsg && lastMsg.role === "ai") {
 // BẮT BUỘC: Phải tạo object mới để tránh bị mutate (React Strict Mode chạy hàm này 2 lần)
 newMsgs[lastMsgIndex] = {
 ...lastMsg,
 content: lastMsg.content + chunk
 };
 }
 return newMsgs;
 });
 }, []);

 	// [User Request: Đã tắt tính năng tự động scroll xuống dòng mới nhất khi AI đang stream]
	// useEffect(() => {
	// 	bottomRef.current?.scrollIntoView({ behavior: "smooth" });
	// }, [messages, isStreaming]);

 const stopStream = useCallback((updateState = true) => {
 if (flushTimerRef.current !== null) {
 window.clearTimeout(flushTimerRef.current);
 flushTimerRef.current = null;
 }
 flushPendingChunk();
 if (eventSourceRef.current) {
 eventSourceRef.current.close();
 eventSourceRef.current = null;
 }
 if (updateState) {
 setIsStreaming(false);
 }
 }, [flushPendingChunk]);

 const startStream = useCallback((customPrompt?: string) => {
 if (!accessToken) {
 setError(t("error_stream"));
 setIsStreaming(false);
 setIsSendingFollowup(false);
 return;
 }

 setIsStreaming(true);
 setError(null);

 const isFollowup = !!customPrompt;
 const messageId = Date.now().toString() + "-ai";

 if (!isFollowup) {
 // Initial reading message
 setMessages([{ id: messageId, role: "ai", content: "", isStreaming: true }]);
 } else {
 // Append Follow-up answer from AI
 setMessages((prev) => [...prev, { id: messageId, role: "ai", content: "", isStreaming: true }]);
 }

 // Khởi tạo Server-Sent Events Connection
 // Thêm tham số `access_token` và `followupQuestion` và `language`
 const baseUrl = `${API_BASE_URL}/sessions/${sessionId}/stream?access_token=${accessToken}&language=${locale}`;
 const finalUrl = customPrompt ? `${baseUrl}&followupQuestion=${encodeURIComponent(customPrompt)}` : baseUrl;

 eventSourceRef.current = new EventSource(finalUrl, {
 withCredentials: true
 });

 eventSourceRef.current.onmessage = (event) => {
 if (event.data === "[DONE]") {
 stopStream();
 if (!isFollowup) setIsComplete(true);
 setIsSendingFollowup(false);

 // Cập nhật lại trạng thái isStreaming = false cho message cuối cùng
 setMessages((prev) => {
 const newMsgs = [...prev];
 const lastMsg = newMsgs[newMsgs.length - 1];
 if (lastMsg && lastMsg.role === "ai") {
 lastMsg.isStreaming = false;
 }
 return newMsgs;
 });

 if (onComplete && !isFollowup) onComplete();
 return;
 }

 try {
 // Xóa Escape char `\n` backend đã encode
 const chunk = event.data.replace(/\\n/g, "\n");
 pendingChunkRef.current += chunk;
 if (flushTimerRef.current === null) {
 flushTimerRef.current = window.setTimeout(() => {
 flushPendingChunk();
 flushTimerRef.current = null;
 }, 48);
 }
	 } catch (err) {
	 console.error("Failed to parse SSE chunk", err);
	 }
	 };

 eventSourceRef.current.onerror = (err) => {
 console.error("SSE Connection Error:", err);
 stopStream();
 setError(t("error_stream"));
 setIsStreaming(false);
 setIsSendingFollowup(false);
 };
 }, [accessToken, flushPendingChunk, onComplete, sessionId, stopStream, t]);

 useEffect(() => {
 if (!accessToken) return;

 const startTimer = window.setTimeout(() => {
 // Dùng ref để đảm bảo chỉ start 1 lần duy nhất cho mỗi sessionId
 if (eventSourceRef.current) return;
 startStream();
 }, 100);

 return () => {
 window.clearTimeout(startTimer);
 stopStream(false);
 };
 // eslint-disable-next-line react-hooks/exhaustive-deps
 }, [accessToken, sessionId]);

 const handleFollowupSubmit = (e: React.FormEvent) => {
 e.preventDefault();
 if (!followupText.trim() || isStreaming || isSendingFollowup) return;

 const question = followupText.trim();
 setFollowupText("");
 setIsSendingFollowup(true);

 // Add user question to UI
 setMessages((prev) => [...prev, { id: Date.now().toString() + "-user", role: "user", content: question }]);

 // Start streaming AI response
 startStream(question);
 };

 	if (!isReadyToShow) {
		return (
			<div className="w-full h-full flex flex-col relative animate-in fade-in duration-1000 px-0 md:px-2 overflow-hidden">
				<div className="flex-1 flex flex-col items-center justify-center text-center space-y-4 opacity-40">
					<RefreshCw className="w-10 h-10 tn-text-muted animate-spin" />
					<p className="tn-text-muted font-serif italic max-w-xs px-4">Đang đợi lật bài...</p>
				</div>
			</div>
		);
	}

	return (
		<div className="w-full h-full flex flex-col relative animate-in fade-in duration-1000 px-0 md:px-2 overflow-hidden">
		{/* Header Removed */}

 {/* Error Message */}
 {error && (
 <div className="m-6 p-4 bg-[var(--danger)]/20 border border-[var(--danger)]/30 rounded-xl flex items-start text-[var(--danger)]">
	 <AlertTriangle className="w-5 h-5 mr-3 mt-0.5 shrink-0" />
	 <div>
	 <p className="font-medium">{t("error_title")}</p>
	 <p className="text-sm opacity-80">{error}</p>
	 <button
	 onClick={() => startStream()}
	 className="mt-3 flex items-center text-sm bg-[var(--danger)]/20 hover:bg-[var(--danger)]/30 px-4 py-2 rounded-lg transition"
	 >
	 <RefreshCw className="w-4 h-4 mr-2" /> {t("error_retry")}
	 </button>
	 </div>
	 </div>
	 )}

 {/* Inner wrapper removed */}

 {/* Scrollable Message Area */}
 <div className="flex-1 overflow-y-auto custom-scrollbar px-3 md:px-6 space-y-6 pt-4 pb-2">
 {messages.length === 0 && !error && isStreaming && (
	 <div className="h-full flex items-center justify-center text-[var(--purple-accent)]/50">
	 <div className="flex flex-col items-center">
	 <Sparkles className="w-10 h-10 animate-pulse mb-4" />
	 <p className="font-serif italic animate-pulse">{t("streaming_placeholder")}</p>
	 </div>
	 </div>
	 )}

 {messages.map((msg) => (
 <div key={msg.id} className={`flex ${msg.role === 'user' ? 'justify-end' : 'justify-start'}`}>
 <div className={`flex max-w-[95%] md:max-w-[92%] ${msg.role === 'user' ? 'flex-row-reverse' : 'flex-row'}`}>

 {/* 
  * [TINH CHỈNH UI]: Đã xóa icon người dùng (User & AI) theo yêu cầu 
  * để nới rộng không gian hiển thị cho đoạn hội thoại (chat bubble).
  * Điều này giúp văn bản của AI chiếm được nhiều diện tích hơn (max-w-[95%] ở mobile, 92% ở desktop).
  */}
 {/* Message Bubble */}
 <div className={`px-6 md:px-8 py-5 rounded-3xl ${msg.role === 'user'
 ? 'bg-[var(--warning)]/10 border border-[var(--warning)]/20 rounded-tr-none'
 : 'bg-[var(--purple-accent)]/10 border border-[var(--purple-accent)]/20 rounded-tl-none prose prose-purple max-w-none prose-p:leading-relaxed prose-p:tn-text-secondary prose-headings:font-serif prose-headings:text-[var(--warning)] prose-strong:text-[var(--purple-accent)] prose-strong:font-bold prose-em:tn-text-secondary prose-em:italic prose-li:tn-text-secondary'}`}>

 {msg.role === 'user' ? (
 <p className="text-[var(--warning)]">{msg.content}</p>
 ) : (
 <ReactMarkdown>{msg.content}</ReactMarkdown>
 )}

 {/* Streaming Indicator */}
 {msg.isStreaming && (
 <span className="inline-flex space-x-1 mt-2">
 <span className="w-1.5 h-1.5 bg-[var(--purple-accent)] rounded-full animate-bounce [animation-delay:-0.3s]"></span>
 <span className="w-1.5 h-1.5 bg-[var(--purple-accent)] rounded-full animate-bounce [animation-delay:-0.15s]"></span>
 <span className="w-1.5 h-1.5 bg-[var(--purple-accent)] rounded-full animate-bounce"></span>
 </span>
 )}
 </div>
 </div>
 </div>
 ))}
 <div ref={bottomRef} className="h-4" />
 </div>

 {/* Follow-up Chat Composer */}
 {isComplete && !isHardCapReached && (
 <form onSubmit={handleFollowupSubmit} className="mt-4 shrink-0 relative animate-in slide-in-from-bottom-5 duration-500">
 <div className="absolute -top-6 left-2 flex items-center gap-2">
 {freeSlotsRemaining > 0 ? (
 <span className="px-2.5 py-1 text-[10px] uppercase font-bold tracking-wider rounded-md bg-[var(--warning)]/20 text-[var(--warning)] border border-[var(--warning)]/30">
 {t("follow_up_free_badge", { remaining: freeSlotsRemaining, total: freeSlotsTotal })}
 </span>
 ) : (
 <span className="px-2.5 py-1 text-[10px] uppercase font-bold tracking-wider rounded-md bg-[var(--purple-accent)]/20 text-[var(--purple-accent)] border border-[var(--purple-accent)]/30 flex items-center">
 <Sparkles className="w-3 h-3 mr-1" />
 {t("follow_up_fee_badge", { cost: nextDiamondCost })}
 </span>
 )}
 </div>
 <div className="relative">
 <input
 type="text"
 value={followupText}
 onChange={(e) => setFollowupText(e.target.value)}
 disabled={isStreaming || isSendingFollowup}
 placeholder={t("follow_up_placeholder")}
 className="w-full tn-field tn-field-accent border-[var(--purple-accent)]/30 tn-text-primary rounded-2xl px-6 py-4 pr-16 disabled:opacity-50 font-serif"
 />
 <button
 type="submit"
 disabled={!followupText.trim() || isStreaming || isSendingFollowup}
 className="absolute right-2 top-2 bottom-2 aspect-square bg-[var(--purple-accent)] hover:bg-[var(--purple-accent)] disabled:tn-surface-strong disabled:tn-text-muted tn-text-primary rounded-xl flex items-center justify-center transition-colors"
 >
 {isSendingFollowup ? (
 <RefreshCw className="w-5 h-5 animate-spin" />
 ) : (
 <Send className="w-5 h-5" />
 )}
 </button>
 </div>
 <p className="text-center text-[10px] tn-text-muted mt-2 font-mono uppercase tracking-wider">
 {t("follow_up_hint")}
 </p>
 </form>
 )}

 {isComplete && isHardCapReached && (
 <div className="mt-4 shrink-0 text-center p-4 tn-surface-strong border tn-border-soft rounded-2xl animate-in slide-in-from-bottom-5 duration-500">
 <p className="text-sm font-medium text-[var(--warning)]">{t("hard_cap_title")}</p>
 <p className="text-xs tn-text-muted mt-1">{t("hard_cap_desc")}</p>
 </div>
 )}
  {/* Duplicate closing tags removed */}
 </div>
 );
}

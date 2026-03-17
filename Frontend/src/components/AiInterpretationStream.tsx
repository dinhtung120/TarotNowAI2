"use client";

import { useEffect, useMemo, useState, useRef } from "react";
import ReactMarkdown from "react-markdown";
import { Sparkles, Bot, AlertTriangle, RefreshCw, Send, User as UserIcon } from "lucide-react";
import { useAuthStore } from "@/store/authStore";
import { useTranslations } from "next-intl";

interface AiInterpretationStreamProps {
 sessionId: string;
 cards?: number[];
 onComplete?: () => void;
}

interface Message {
 id: string;
 role: "ai" | "user";
 content: string;
 isStreaming?: boolean;
}

export default function AiInterpretationStream({ sessionId, cards, onComplete }: AiInterpretationStreamProps) {
 const accessToken = useAuthStore((state) => state.accessToken);
 const t = useTranslations("AiInterpretation");
 const [messages, setMessages] = useState<Message[]>([]);

 // Initial reading states
 const [isStreaming, setIsStreaming] = useState(false);
 const [error, setError] = useState<string | null>(null);
 const [isComplete, setIsComplete] = useState(false);
 const eventSourceRef = useRef<EventSource | null>(null);
 const bottomRef = useRef<HTMLDivElement>(null);

 // Follow-up interaction states
 const [followupText, setFollowupText] = useState("");
 const [isSendingFollowup, setIsSendingFollowup] = useState(false);

 // Follow-up pricing states
 const freeSlotsTotal = useMemo(() => {
 if (!cards || cards.length === 0) return 0;

 let highestLevel = 1;
 cards.forEach(cardId => {
 // This mirrors BE logic: Id 0-21 are Major Arcana.
 // Simplified level mapping: 0-9 -> 1-10 level, 10-19 -> 11-20 level. // In a real app the exact logic should match BE or be passed by BE directly.
 // We use a simplified approximation here for MVP UI.
 if (cardId < 22) {
 const calculatedLvl = cardId < 10 ? cardId + 1 : cardId < 20 ? cardId - 5 : 20;
 if (calculatedLvl > highestLevel) highestLevel = calculatedLvl;
 }
 });

 if (highestLevel >= 16) return 3;
 if (highestLevel >= 11) return 2;
 if (highestLevel >= 6) return 1;
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

 // Auto scroll bottom when new message arrives or streaming
 useEffect(() => {
 bottomRef.current?.scrollIntoView({ behavior: "smooth" });
 }, [messages, isStreaming]);

 const stopStream = (updateState = true) => {
 if (eventSourceRef.current) {
 eventSourceRef.current.close();
 eventSourceRef.current = null;
 }
 if (updateState) {
 setIsStreaming(false);
 }
 };

 const startStream = (customPrompt?: string) => {
 setIsStreaming(true);
 setError(null);

 const isFollowup = !!customPrompt;
 const messageId = Date.now().toString();

 if (!isFollowup) {
 // Initial reading message
 setMessages([{ id: messageId, role: "ai", content: "", isStreaming: true }]);
 } else {
 // Append Follow-up answer from AI
 setMessages((prev) => [...prev, { id: messageId, role: "ai", content: "", isStreaming: true }]);
 }

 // Khởi tạo Server-Sent Events Connection
 // Thêm tham số `access_token` và `followupQuestion` (nếu có)
 const apiUrl = process.env.NEXT_PUBLIC_API_URL || "http://localhost:5221/api/v1";
 const baseUrl = `${apiUrl}/sessions/${sessionId}/stream?access_token=${accessToken}`;
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

 // Cập nhật State kiểu Typewriter cho message cuối
 setMessages((prev) => {
 const newMsgs = [...prev];
 const lastMsg = newMsgs[newMsgs.length - 1];
 if (lastMsg && lastMsg.role === "ai") {
 lastMsg.content += chunk;
 }
 return newMsgs;
 });
 } catch (err) {
 console.error("Lỗi parse SSE Chunk", err);
 }
 };

 eventSourceRef.current.onerror = (err) => {
 console.error("SSE Connection Error:", err);
 stopStream();
 setError(t("error_stream"));
 setIsStreaming(false);
 setIsSendingFollowup(false);
 };
 };

 useEffect(() => {
 const startTimer = window.setTimeout(() => {
 if (eventSourceRef.current || isComplete || error || messages.length > 0) return;
 startStream();
 }, 0);

 return () => {
 window.clearTimeout(startTimer);
 stopStream(false);
 };
 // eslint-disable-next-line react-hooks/exhaustive-deps
 }, [sessionId]);

 const handleFollowupSubmit = (e: React.FormEvent) => {
 e.preventDefault();
 if (!followupText.trim() || isStreaming || isSendingFollowup) return;

 const question = followupText.trim();
 setFollowupText("");
 setIsSendingFollowup(true);

 // Add user question to UI
 setMessages((prev) => [...prev, { id: Date.now().toString(), role: "user", content: question }]);

 // Start streaming AI response
 startStream(question);
 };

 return (
 <div className="w-full max-w-4xl mx-auto mt-16 tn-overlay rounded-3xl border border-[var(--purple-accent)]/20 shadow-2xl overflow-hidden animate-in fade-in slide-in-from-bottom-10 duration-1000">
 {/* Header / Title */}
 <div className="tn-grad-ai-header p-6 border-b border-[var(--purple-accent)]/20 flex flex-col md:flex-row md:items-center justify-between gap-4">
 <div className="flex items-center">
 <div className="w-12 h-12 bg-[var(--purple-accent)]/20 rounded-full flex items-center justify-center mr-4">
 <Bot className="w-6 h-6 text-[var(--purple-accent)]" />
 </div>
 <div>
 <h3 className="text-xl font-serif font-bold tn-text-primary flex items-center">
 {t("title")}
 {isStreaming && (
 <span className="ml-3 flex space-x-1">
 <span className="w-2 h-2 bg-[var(--purple-accent)] rounded-full animate-bounce [animation-delay:-0.3s]"></span>
 <span className="w-2 h-2 bg-[var(--purple-accent)] rounded-full animate-bounce [animation-delay:-0.15s]"></span>
 <span className="w-2 h-2 bg-[var(--purple-accent)] rounded-full animate-bounce"></span>
 </span>
 )}
 </h3>
 <p className="text-sm tn-text-secondary">{t("subtitle")}</p>
 </div>
 </div>

 {isComplete && (
 <div className="flex items-center text-[var(--warning)] text-sm font-medium bg-[var(--warning)]/10 px-4 py-2 rounded-full border border-[var(--warning)]/20">
 <Sparkles className="w-4 h-4 mr-2" /> {t("status_complete")}
 </div>
 )}
 </div>

 {/* Error Message */}
 {error && (
 <div className="m-6 p-4 bg-[var(--danger)]/20 border border-[var(--danger)]/30 rounded-xl flex items-start text-[var(--danger)]">
 <AlertTriangle className="w-5 h-5 mr-3 mt-0.5 shrink-0" />
 <div>
 <p className="font-medium">Lỗi Năng Lượng Thần Giao</p>
 <p className="text-sm opacity-80">{error}</p>
 <button
 onClick={() => startStream()}
 className="mt-3 flex items-center text-sm bg-[var(--danger)]/20 hover:bg-[var(--danger)]/30 px-4 py-2 rounded-lg transition"
 >
 <RefreshCw className="w-4 h-4 mr-2" /> Thử Kênh Khác (Refund Tự Động)
 </button>
 </div>
 </div>
 )}

 {/* Content Body (Chat Interface) */}
 <div className="p-6 h-[400px] md:h-[500px] flex flex-col relative">

 {/* Scrollable Message Area */}
 <div className="flex-1 overflow-y-auto custom-scrollbar pr-2 space-y-6">
 {messages.length === 0 && !error && isStreaming && (
 <div className="h-full flex items-center justify-center text-[var(--purple-accent)]/50">
 <div className="flex flex-col items-center">
 <Sparkles className="w-10 h-10 animate-pulse mb-4" />
 <p className="font-serif italic animate-pulse">Các Tinh Tú Đang Thẳng Hàng...</p>
 </div>
 </div>
 )}

 {messages.map((msg) => (
 <div key={msg.id} className={`flex ${msg.role === 'user' ? 'justify-end' : 'justify-start'}`}>
 <div className={`flex max-w-[90%] md:max-w-[85%] gap-4 ${msg.role === 'user' ? 'flex-row-reverse' : 'flex-row'}`}>

 {/* Avatar */}
 <div className={`shrink-0 w-10 h-10 rounded-full flex items-center justify-center border
 ${msg.role === 'user'
 ? 'bg-[var(--warning)]/40 border-[var(--warning)]/30 text-[var(--warning)]'
 : 'bg-[var(--purple-accent)]/40 border-[var(--purple-accent)]/30 text-[var(--purple-accent)]'}`}>
 {msg.role === 'user' ? <UserIcon className="w-5 h-5" /> : <Bot className="w-5 h-5" />}
 </div>

 {/* Message Bubble */}
 <div className={`px-5 py-4 rounded-3xl ${msg.role === 'user'
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
 Miễn Phí Follow-up ({freeSlotsRemaining}/{freeSlotsTotal})
 </span>
 ) : (
 <span className="px-2.5 py-1 text-[10px] uppercase font-bold tracking-wider rounded-md bg-[var(--purple-accent)]/20 text-[var(--purple-accent)] border border-[var(--purple-accent)]/30 flex items-center">
 <Sparkles className="w-3 h-3 mr-1" />
 Phí: {nextDiamondCost} Diamond
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
 Giới hạn 5 câu hỏi phụ. Tốn AI Quota nhưng miễn phí Diamond nếu bài đủ cấp.
 </p>
 </form>
 )}

 {isComplete && isHardCapReached && (
 <div className="mt-4 shrink-0 text-center p-4 tn-surface-strong border tn-border-soft rounded-2xl animate-in slide-in-from-bottom-5 duration-500">
 <p className="text-sm font-medium text-[var(--warning)]">Giới hạn Vũ Trụ đã đạt</p>
 <p className="text-xs tn-text-muted mt-1">Năng lượng của dải ngân hà trong phiên này đã vơi cạn (5/5 câu). Quá nhiều sự phân tâm sẽ làm nhiễu loạn thông điệp ban đầu. Xin hãy đón nhận và chiêm nghiệm tinh hoa trên.</p>
 </div>
 )}
 </div>

 </div>
 );
}

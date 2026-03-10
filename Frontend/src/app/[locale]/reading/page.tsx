"use client";

import { useState } from "react";

import { useRouter } from "@/i18n/routing";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import * as z from "zod";
import { initReadingSession } from "@/actions/readingActions";
import { Loader2, Sparkles, AlertCircle } from "lucide-react";
import { useWalletStore } from "@/store/walletStore";

/**
 * Zod Schema validated form (Optional message)
 */
const formSchema = z.object({
    question: z.string().max(300, "Question is too long").optional(),
});

type FormData = z.infer<typeof formSchema>;

export default function ReadingSetupPage() {
    const router = useRouter();
    const { fetchBalance } = useWalletStore(); // Reload wallet after init
    const [selectedSpread, setSelectedSpread] = useState<string>("daily_1");
    const [initError, setInitError] = useState("");
    const [isInitializing, setIsInitializing] = useState(false);

    // Form setup
    const { register, handleSubmit, formState: { errors } } = useForm<FormData>({
        resolver: zodResolver(formSchema),
    });

    const SPREADS = [
        { id: "daily_1", name: "Daily 1 Card", desc: "1 lá bài chỉ dẫn mỗi ngày (Miễn phí)", cost: "Free", icon: "☀️" },
        { id: "spread_3", name: "Past, Present, Future", desc: "Trải bài 3 lá kinh điển", cost: "50 Gold", icon: "⏳" },
        { id: "spread_5", name: "Cross Spread", desc: "Trải bài 5 lá phân tích tình huống", cost: "100 Gold", icon: "✖️" },
        { id: "spread_10", name: "Celtic Cross", desc: "Trải bài 10 lá chuyên sâu", cost: "50 Diamond", icon: "🔮" },
    ];

    const onSubmit = async (data: FormData) => {
        setIsInitializing(true);
        setInitError("");

        const response = await initReadingSession({
            spreadType: selectedSpread
        });

        if (response.success && response.data) {
            // Refresh wallet balance do đã bị trừ tiền
            await fetchBalance();

            const cardsToDrawMap: Record<string, number> = {
                "daily_1": 1,
                "spread_3": 3,
                "spread_5": 5,
                "spread_10": 10,
            };
            const cardsToDraw = cardsToDrawMap[selectedSpread] || 1;

            // Lưu thông tin vào Session Storage để truyền sang trang rút bài
            if (data.question) {
                sessionStorage.setItem(`question_${response.data.sessionId}`, data.question);
            }
            sessionStorage.setItem(`cardsToDraw_${response.data.sessionId}`, cardsToDraw.toString());

            // Redirect sang phòng rút
            router.push(`/reading/session/${response.data.sessionId}`);
        } else {
            setInitError(response.error || "Failed to initialize reading session.");
            setIsInitializing(false);
        }
    };

    return (
        <div className="min-h-screen bg-black text-white p-6 pt-24">
            <div className="max-w-4xl mx-auto">
                <div className="text-center mb-12">
                    <h1 className="text-4xl md:text-5xl font-bold bg-gradient-to-r from-purple-400 via-pink-400 to-amber-400 text-transparent bg-clip-text mb-4">
                        Select Your Spread
                    </h1>
                    <p className="text-gray-400 text-lg">
                        Chọn một kiểu trải bài và đặt câu hỏi để nhận thông điệp từ vũ trụ
                    </p>
                </div>

                {initError && (
                    <div className="mb-8 p-4 bg-red-900/50 border border-red-500/50 rounded-xl flex items-start gap-3 text-red-200">
                        <AlertCircle className="w-5 h-5 flex-shrink-0 mt-0.5" />
                        <p className="text-sm">{initError}</p>
                    </div>
                )}

                <form onSubmit={handleSubmit(onSubmit)} className="space-y-8">

                    {/* Spread Selection Grid */}
                    <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                        {SPREADS.map((spread) => (
                            <div
                                key={spread.id}
                                onClick={() => setSelectedSpread(spread.id)}
                                className={`relative p-6 rounded-2xl cursor-pointer transition-all duration-300 border ${selectedSpread === spread.id
                                    ? "bg-purple-900/30 border-purple-500 shadow-[0_0_30px_rgba(168,85,247,0.2)]"
                                    : "bg-zinc-900/50 border-zinc-800 hover:border-zinc-700 hover:bg-zinc-800/50"
                                    }`}
                            >
                                <div className="flex justify-between items-start mb-4">
                                    <div className="text-3xl">{spread.icon}</div>
                                    <span className={`px-3 py-1 rounded-full text-xs font-medium tracking-wide ${spread.cost === 'Free' ? 'bg-green-500/20 text-green-300 border border-green-500/30' :
                                        spread.cost.includes('Diamond') ? 'bg-blue-500/20 text-blue-300 border border-blue-500/30' :
                                            'bg-amber-500/20 text-amber-300 border border-amber-500/30'
                                        }`}>
                                        {spread.cost}
                                    </span>
                                </div>
                                <h3 className="text-xl font-semibold mb-2 text-zinc-100">{spread.name}</h3>
                                <p className="text-zinc-400 text-sm">{spread.desc}</p>

                                {selectedSpread === spread.id && (
                                    <div className="absolute inset-0 border-2 border-purple-500 rounded-2xl animate-pulse"></div>
                                )}
                            </div>
                        ))}
                    </div>

                    {/* Question Input */}
                    <div className="bg-zinc-900/30 p-6 rounded-2xl border border-zinc-800 backdrop-blur-sm">
                        <label htmlFor="question" className="block text-sm font-medium text-zinc-300 mb-2">
                            Deepen Your Reading (Optional)
                        </label>
                        <textarea
                            id="question"
                            rows={3}
                            {...register("question")}
                            placeholder="Concentrate on your situation and type your question here..."
                            className="w-full bg-black/50 border border-zinc-800 rounded-xl px-4 py-3 text-white placeholder-zinc-600 focus:outline-none focus:ring-2 focus:ring-purple-500/50 resize-none transition-all"
                        />
                        {errors.question && (
                            <p className="mt-2 text-sm text-red-400">{errors.question.message}</p>
                        )}
                    </div>

                    {/* Submit Button */}
                    <div className="flex justify-center pt-4">
                        <button
                            type="submit"
                            disabled={isInitializing}
                            className="group relative inline-flex items-center justify-center px-8 py-4 font-bold text-white transition-all duration-200 bg-gradient-to-r from-purple-600 to-pink-600 font-pj rounded-xl focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-gray-900 disabled:opacity-50 disabled:cursor-not-allowed hover:from-purple-500 hover:to-pink-500 shadow-[0_0_40px_rgba(168,85,247,0.4)] hover:shadow-[0_0_60px_rgba(168,85,247,0.6)] overflow-hidden"
                        >
                            {isInitializing ? (
                                <Loader2 className="w-5 h-5 animate-spin mr-2" />
                            ) : (
                                <Sparkles className="w-5 h-5 mr-2 group-hover:animate-ping" />
                            )}
                            {isInitializing ? "Preparing the cards..." : "Draw Cards"}
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
}

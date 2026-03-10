"use client";

import { useEffect, useState } from "react";
import { getUserCollection, UserCollectionDto } from "@/actions/collectionActions";
import { TAROT_DECK } from "@/lib/tarotData";
import { Loader2, LayoutGrid, AlertCircle, ChevronRight } from "lucide-react";
import { Link } from "@/i18n/routing";

export default function CollectionPage() {
    const [collection, setCollection] = useState<UserCollectionDto[]>([]);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState("");

    useEffect(() => {
        const fetchCollection = async () => {
            const result = await getUserCollection();
            if (result.success && result.data) {
                setCollection(result.data);
            } else {
                setError(result.error || "Failed to load collection");
            }
            setIsLoading(false);
        };
        fetchCollection();
    }, []);

    if (isLoading) {
        return (
            <div className="min-h-screen bg-black flex items-center justify-center">
                <Loader2 className="w-10 h-10 animate-spin text-purple-500" />
            </div>
        );
    }

    // Lấy ra tỉ lệ sưu tập
    const totalCollected = collection.length;
    const progressRatio = (totalCollected / 78) * 100;

    return (
        <div className="min-h-screen bg-black text-white p-6 pt-24 pb-20">
            <div className="max-w-7xl mx-auto">
                <div className="flex flex-col md:flex-row items-center justify-between gap-6 mb-12">
                    <div>
                        <h1 className="text-4xl font-bold bg-gradient-to-r from-amber-400 via-yellow-300 to-orange-400 text-transparent bg-clip-text flex items-center gap-3">
                            <LayoutGrid className="w-8 h-8 text-amber-500" /> Miếu Bài Thần Bí
                        </h1>
                        <p className="text-zinc-400 mt-2">Dõi theo tiến độ khai mở vũ trụ lá bài Tarot của bạn.</p>
                    </div>

                    <div className="bg-zinc-900/50 p-4 rounded-2xl border border-zinc-800 flex items-center gap-6 min-w-[300px]">
                        <div className="flex-1">
                            <div className="flex justify-between text-sm mb-2">
                                <span className="text-zinc-400">Tiến độ Sưu Tầm</span>
                                <span className="font-bold text-amber-400">{totalCollected} / 78</span>
                            </div>
                            <div className="w-full h-2 bg-zinc-800 rounded-full overflow-hidden">
                                <div
                                    className="h-full bg-gradient-to-r from-amber-500 to-yellow-400 rounded-full transition-all duration-1000"
                                    style={{ width: `${progressRatio}%` }}
                                />
                            </div>
                        </div>
                    </div>
                </div>

                {error && (
                    <div className="mb-8 p-4 bg-red-900/50 border border-red-500/50 rounded-xl flex items-start gap-3 text-red-200">
                        <AlertCircle className="w-5 h-5 flex-shrink-0 mt-0.5" />
                        <p className="text-sm">{error}</p>
                    </div>
                )}

                {collection.length === 0 && !error ? (
                    <div className="text-center py-20 bg-zinc-900/30 border border-zinc-800 rounded-3xl">
                        <h3 className="text-xl text-zinc-300 mb-4">Kho bài của bạn đang trống</h3>
                        <p className="text-zinc-500 mb-8">Hãy thực hiện phiên rút Tarot đầu tiên để mở khóa thẻ bài.</p>
                        <Link href="/reading" className="px-6 py-3 bg-purple-600 hover:bg-purple-500 text-white rounded-xl transition inline-flex items-center gap-2">
                            Rút Thuật Ngay <ChevronRight className="w-4 h-4" />
                        </Link>
                    </div>
                ) : (
                    <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 xl:grid-cols-6 gap-6">
                        {/* Map tất cả thẻ bài Tarot (Màu mờ nếu chưa có, sáng nếu đã sở hữu) */}
                        {TAROT_DECK.map((deckCard) => {
                            const userCard = collection.find(c => c.cardId === deckCard.id);
                            const isOwned = !!userCard;

                            return (
                                <div
                                    key={deckCard.id}
                                    className={`relative group rounded-2xl p-4 flex flex-col items-center transition-all duration-300 border ${isOwned
                                        ? "bg-zinc-900/60 border-amber-500/30 hover:border-amber-400 hover:bg-zinc-800"
                                        : "bg-black border-zinc-900 opacity-40 grayscale hover:grayscale-0 hover:opacity-100"
                                        }`}
                                >
                                    <div className="w-full text-center mb-3">
                                        <div className={`text-[10px] font-bold uppercase tracking-wider ${isOwned ? 'text-amber-500' : 'text-zinc-600'}`}>
                                            {deckCard.suit.split(' ')[0]} {/* Lấy Major hay Minor */}
                                        </div>
                                    </div>

                                    <div className={`w-full aspect-[2/3] rounded-xl mb-4 flex items-center justify-center relative overflow-hidden ${isOwned ? 'bg-gradient-to-br from-zinc-800 to-zinc-900 shadow-inner' : 'bg-zinc-900'}`}>
                                        <span className={`text-4xl font-serif drop-shadow-md ${isOwned ? 'text-amber-100' : 'text-zinc-700'}`}>
                                            {deckCard.id + 1}
                                        </span>
                                        {isOwned && (
                                            <div className="absolute inset-0 bg-gradient-to-t from-purple-900/50 to-transparent"></div>
                                        )}
                                    </div>

                                    <h4 className={`text-sm font-bold text-center leading-tight mb-2 ${isOwned ? 'text-zinc-100' : 'text-zinc-500'}`}>
                                        {deckCard.name}
                                    </h4>

                                    {isOwned ? (
                                        <div className="w-full mt-auto">
                                            <div className="flex justify-between items-center text-xs text-zinc-400 mb-1">
                                                <span>Lv. {userCard.level}</span>
                                                <span>{userCard.copies} bản</span>
                                            </div>
                                            <div className="w-full h-1 bg-zinc-800 rounded-full">
                                                <div className="h-full bg-purple-500 rounded-full" style={{ width: `${(userCard.copies % 5) * 20}%` }}></div>
                                            </div>
                                        </div>
                                    ) : (
                                        <div className="w-full mt-auto text-center">
                                            <span className="text-xs text-zinc-600">Chưa Khám Phá</span>
                                        </div>
                                    )}
                                </div>
                            );
                        })}
                    </div>
                )}
            </div>
        </div>
    );
}

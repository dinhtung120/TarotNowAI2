"use client";

import { Gift } from "lucide-react";
import { toast } from "react-hot-toast";
import { useQueryClient } from "@tanstack/react-query";
import { useWalletStore } from "@/store/walletStore";
import { useClaimQuestReward } from "@/features/gamification/useGamification";

interface UseQuestClaimHandlersParams {
 t: (key: string, values?: Record<string, string | number>) => string;
}

export function useQuestClaimHandlers({ t }: UseQuestClaimHandlersParams) {
 const queryClient = useQueryClient();
 const claimMutation = useClaimQuestReward();

 const getClaimErrorMessage = (error: unknown) => {
  if (error instanceof Error && error.message) return error.message;
  if (typeof error === "object" && error !== null) {
   const maybeHttpError = error as { response?: { data?: { message?: string } } };
   if (maybeHttpError.response?.data?.message) return maybeHttpError.response.data.message;
  }
  return t("ClaimFailed");
 };

 const handleClaim = (questCode: string, periodKey: string) => {
  claimMutation.mutate({ questCode, periodKey }, { onSuccess: (result) => { if (!result.success) return; queryClient.invalidateQueries({ queryKey: ["wallet", "balance"] }); queryClient.invalidateQueries({ queryKey: ["gamification", "quests"] }); useWalletStore.getState().fetchBalance(); toast.success(<div className="flex items-center gap-2"><Gift className="text-yellow-400 w-5 h-5" /><span>{t("ClaimedSuccessfully", { amount: result.rewardAmount, type: result.rewardType })}</span></div>, { style: { borderRadius: "16px", background: "rgba(30, 41, 59, 0.9)", color: "#fff", border: "1px solid rgba(255, 255, 255, 0.1)", backdropFilter: "blur(10px)" } }); }, onError: (error: unknown) => toast.error(getClaimErrorMessage(error)) });
 };

 return { claimMutation, handleClaim };
}

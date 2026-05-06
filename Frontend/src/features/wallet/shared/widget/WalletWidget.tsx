"use client";

import { WalletWidgetContent } from "@/features/wallet/shared/widget/wallet-widget/WalletWidgetContent";
import { WalletWidgetLoading } from "@/features/wallet/shared/widget/wallet-widget/WalletWidgetLoading";
import { useWalletWidgetState } from "@/features/wallet/shared/widget/wallet-widget/useWalletWidgetState";

export default function WalletWidget() {
 const state = useWalletWidgetState();
 if (state.isLoading) return <WalletWidgetLoading />;
 if (!state.balance) return null;

 return (
  <WalletWidgetContent
   locale={state.locale}
   title={state.t("widget.title")}
   diamondTitle={state.t("widget.diamond_title")}
   goldTitle={state.t("widget.gold_title")}
   frozenSuffix={(amount) => state.t("widget.frozen_suffix", { amount })}
   balance={state.balance}
  />
 );
}

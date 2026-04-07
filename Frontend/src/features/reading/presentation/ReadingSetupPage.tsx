"use client";

import { Compass, Sparkles } from "lucide-react";
import { useReadingSetupPage } from "@/features/reading/application/useReadingSetupPage";
import { Input, SectionHeader } from "@/shared/components/ui";
import { ReadingCurrencySelector, ReadingSetupSubmitAction, ReadingSpreadsGrid } from "@/features/reading/presentation/components/reading-setup";

export default function ReadingSetupPage() {
 const vm = useReadingSetupPage();
 const submitHandler = vm.handleSubmit(vm.submitSetup);

 return (
  <div className="max-w-4xl mx-auto px-4 sm:px-6 pt-10 pb-20 font-sans">
   <SectionHeader title={vm.t("title")} subtitle={vm.t("subtitle")} tag={vm.t("tag")} tagIcon={<Compass className="w-3 h-3" />} className="mb-10" />
   {vm.initError ? <div className="mb-8 p-4 bg-[var(--danger)]/10 border border-[var(--danger)]/20 rounded-xl text-[var(--danger)] text-sm">{vm.initError}</div> : null}
   <ReadingCurrencySelector selectedCurrency={vm.selectedCurrency} labels={{ title: vm.t("select_currency"), gold: vm.t("currency_gold"), diamond: vm.t("currency_diamond") }} onSelectCurrency={vm.setSelectedCurrency} />
   <form onSubmit={submitHandler} className="space-y-10">
    <ReadingSpreadsGrid spreads={vm.spreads} selectedSpread={vm.selectedSpread} selectedCurrency={vm.selectedCurrency} expBonusLabel={(amount) => vm.t("exp_bonus", { amount })} onSelectSpread={vm.setSelectedSpread} />
    {vm.selectedSpread !== "daily_1" ? (
     <Input label={vm.t("question_label")} isTextarea placeholder={vm.t("question_placeholder")} leftIcon={<Sparkles className="w-5 h-5 text-[var(--purple-accent)]" />} error={vm.errors.question?.message} {...vm.register("question")} />
    ) : null}
    <ReadingSetupSubmitAction isInitializing={vm.isInitializing} preparingLabel={vm.t("preparing")} submitLabel={vm.t("cta_draw")} />
   </form>
  </div>
 );
}

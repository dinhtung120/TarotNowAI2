"use client";

import { Compass, Sparkles } from "lucide-react";
import { useReadingSetupPage } from "@/features/reading/application/useReadingSetupPage";
import { Input, SectionHeader } from "@/shared/components/ui";
import { ReadingCurrencySelector, ReadingSetupSubmitAction, ReadingSpreadsGrid } from "@/features/reading/presentation/components/reading-setup";
import { cn } from "@/lib/utils";

export default function ReadingSetupPage() {
 const vm = useReadingSetupPage();
 const submitHandler = vm.handleSubmit(vm.submitSetup);

 return (
  <div className={cn("mx-auto", "max-w-4xl", "px-6", "pt-10", "pb-20", "font-sans")}>
   <SectionHeader title={vm.t("title")} subtitle={vm.t("subtitle")} tag={vm.t("tag")} tagIcon={<Compass className={cn("h-3", "w-3")} />} className={cn("mb-10")} />
   {vm.initError ? <div className={cn("mb-8", "rounded-xl", "border", "border-red-500/20", "bg-red-500/10", "p-4", "text-sm", "text-red-400")}>{vm.initError}</div> : null}
   <ReadingCurrencySelector selectedCurrency={vm.selectedCurrency} labels={{ title: vm.t("select_currency"), gold: vm.t("currency_gold"), diamond: vm.t("currency_diamond") }} onSelectCurrency={vm.setSelectedCurrency} />
   <form onSubmit={submitHandler} className={cn("space-y-10")}>
    <ReadingSpreadsGrid spreads={vm.spreads} selectedSpread={vm.selectedSpread} selectedCurrency={vm.selectedCurrency} expBonusLabel={(amount) => vm.t("exp_bonus", { amount })} onSelectSpread={vm.setSelectedSpread} />
    {vm.selectedSpread !== "daily_1" ? (
     <Input label={vm.t("question_label")} isTextarea placeholder={vm.t("question_placeholder")} leftIcon={<Sparkles className={cn("h-5", "w-5", "text-violet-400")} />} error={vm.errors.question?.message} {...vm.register("question")} />
    ) : null}
    <ReadingSetupSubmitAction isInitializing={vm.isInitializing} preparingLabel={vm.t("preparing")} submitLabel={vm.t("cta_draw")} />
   </form>
  </div>
 );
}

'use client';

import { useEffect } from "react";
import type { FormEvent } from "react";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm, useWatch } from "react-hook-form";
import { z } from "zod";
import { GlassCard } from "@/shared/components/ui";
import { cn } from "@/lib/utils";
import { WithdrawAmountSection } from "./WithdrawAmountSection";
import { WithdrawBankFields } from "./WithdrawBankFields";
import { WithdrawSubmitSection } from "./WithdrawSubmitSection";
import type { WithdrawFormCardProps } from "./WithdrawFormCard.types";

const withdrawFormCardSchema = z.object({
  amount: z
    .string()
    .trim()
    .min(1)
    .refine((value) => {
      const parsed = Number.parseInt(value, 10);
      return Number.isFinite(parsed) && parsed >= 50;
    }),
  bankName: z.string().trim().min(1).max(120),
  accountName: z.string().trim().min(1).max(120),
  accountNumber: z.string().trim().min(1).max(64),
});

type WithdrawFormCardFormValues = z.infer<typeof withdrawFormCardSchema>;

export function WithdrawFormCard(props: WithdrawFormCardProps) {
  const {
    onAccountNameChange,
    onAccountNumberChange,
    onAmountChange,
    onBankNameChange,
    onSubmit,
  } = props;
  const { handleSubmit, setValue, control } = useForm<WithdrawFormCardFormValues>({
    resolver: zodResolver(withdrawFormCardSchema),
    defaultValues: {
      amount: props.amount,
      bankName: props.bankName,
      accountName: props.accountName,
      accountNumber: props.accountNumber,
    },
  });

  const watchedAmount = useWatch({ control, name: "amount" }) ?? "";
  const watchedBankName = useWatch({ control, name: "bankName" }) ?? "";
  const watchedAccountName = useWatch({ control, name: "accountName" }) ?? "";
  const watchedAccountNumber = useWatch({ control, name: "accountNumber" }) ?? "";

  useEffect(() => {
    setValue("amount", props.amount, { shouldDirty: false, shouldValidate: false });
  }, [props.amount, setValue]);

  useEffect(() => {
    setValue("bankName", props.bankName, { shouldDirty: false, shouldValidate: false });
  }, [props.bankName, setValue]);

  useEffect(() => {
    setValue("accountName", props.accountName, {
      shouldDirty: false,
      shouldValidate: false,
    });
  }, [props.accountName, setValue]);

  useEffect(() => {
    setValue("accountNumber", props.accountNumber, {
      shouldDirty: false,
      shouldValidate: false,
    });
  }, [props.accountNumber, setValue]);

  useEffect(() => {
    onAmountChange(watchedAmount);
  }, [onAmountChange, watchedAmount]);

  useEffect(() => {
    onBankNameChange(watchedBankName);
  }, [onBankNameChange, watchedBankName]);

  useEffect(() => {
    onAccountNameChange(watchedAccountName);
  }, [onAccountNameChange, watchedAccountName]);

  useEffect(() => {
    onAccountNumberChange(watchedAccountNumber);
  }, [onAccountNumberChange, watchedAccountNumber]);

  const submitWithValidation = handleSubmit(() => {
    onSubmit({
      preventDefault: () => undefined,
      stopPropagation: () => undefined,
    } as unknown as FormEvent<HTMLFormElement>);
  });

  return (
    <GlassCard
      className={cn(
        "animate-in fade-in slide-in-from-bottom-8 delay-200 duration-1000",
      )}
    >
      <form className={cn("space-y-6")} onSubmit={submitWithValidation}>
        <WithdrawAmountSection
          amount={watchedAmount}
          amountNum={props.amountNum}
          feeVnd={props.feeVnd}
          grossVnd={props.grossVnd}
          labels={{
            amountLabel: props.labels.amountLabel,
            amountPlaceholder: props.labels.amountPlaceholder,
            grossLabel: props.labels.grossLabel,
            feeLabel: props.labels.feeLabel,
            netLabel: props.labels.netLabel,
          }}
          locale={props.locale}
          netVnd={props.netVnd}
          onAmountChange={(value) =>
            setValue("amount", value, { shouldDirty: true, shouldValidate: true })
          }
        />
        <WithdrawBankFields
          accountName={watchedAccountName}
          accountNumber={watchedAccountNumber}
          bankName={watchedBankName}
          labels={{
            bankLabel: props.labels.bankLabel,
            bankPlaceholder: props.labels.bankPlaceholder,
            accountNameLabel: props.labels.accountNameLabel,
            accountNamePlaceholder: props.labels.accountNamePlaceholder,
            accountNumberLabel: props.labels.accountNumberLabel,
            accountNumberPlaceholder: props.labels.accountNumberPlaceholder,
          }}
          onAccountNameChange={(value) =>
            setValue("accountName", value, { shouldDirty: true, shouldValidate: true })
          }
          onAccountNumberChange={(value) =>
            setValue("accountNumber", value, {
              shouldDirty: true,
              shouldValidate: true,
            })
          }
          onBankNameChange={(value) =>
            setValue("bankName", value, { shouldDirty: true, shouldValidate: true })
          }
        />
        <WithdrawSubmitSection
          amountNum={props.amountNum}
          error={props.error}
          submitLabel={props.labels.submitLabel}
          submitting={props.submitting}
          submittingLabel={props.labels.submittingLabel}
          success={props.success}
          successMessage={props.labels.successMessage}
        />
      </form>
    </GlassCard>
  );
}

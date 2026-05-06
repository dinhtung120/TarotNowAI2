import { z } from 'zod';
import type {
 AdminAchievementDefinition,
 AdminQuestDefinition,
 AdminTitleDefinition,
} from '@/features/admin/gamification/types/adminGamification.types';

const nonEmptyText = z.string().trim().min(1);
const optionalText = z.string().trim();

export const adminQuestFormSchema = z.object({
 code: nonEmptyText,
 titleVi: nonEmptyText,
 titleEn: nonEmptyText,
 titleZh: nonEmptyText,
 descriptionVi: nonEmptyText,
 descriptionEn: nonEmptyText,
 descriptionZh: nonEmptyText,
 questType: nonEmptyText,
 triggerEvent: nonEmptyText,
 target: z.number().int().min(1),
 isActive: z.boolean(),
 rewards: z.array(z.object({
  type: nonEmptyText,
  amount: z.number().int().min(1),
  titleCode: optionalText,
 })).min(1),
});

export const adminAchievementFormSchema = z.object({
 code: nonEmptyText,
 titleVi: nonEmptyText,
 titleEn: nonEmptyText,
 titleZh: nonEmptyText,
 descriptionVi: nonEmptyText,
 descriptionEn: nonEmptyText,
 descriptionZh: nonEmptyText,
 icon: optionalText,
 grantsTitleCode: optionalText,
 isActive: z.boolean(),
});

export const adminTitleFormSchema = z.object({
 code: nonEmptyText,
 nameVi: nonEmptyText,
 nameEn: nonEmptyText,
 nameZh: nonEmptyText,
 descriptionVi: nonEmptyText,
 descriptionEn: nonEmptyText,
 descriptionZh: nonEmptyText,
 rarity: nonEmptyText,
 isActive: z.boolean(),
});

export type AdminQuestFormValues = z.infer<typeof adminQuestFormSchema>;
export type AdminAchievementFormValues = z.infer<typeof adminAchievementFormSchema>;
export type AdminTitleFormValues = z.infer<typeof adminTitleFormSchema>;

export function toQuestFormValues(value?: AdminQuestDefinition | null): AdminQuestFormValues {
 return {
  code: value?.code ?? '',
  titleVi: value?.titleVi ?? '',
  titleEn: value?.titleEn ?? '',
  titleZh: value?.titleZh ?? '',
  descriptionVi: value?.descriptionVi ?? '',
  descriptionEn: value?.descriptionEn ?? '',
  descriptionZh: value?.descriptionZh ?? '',
  questType: value?.questType ?? '',
  triggerEvent: value?.triggerEvent ?? '',
  target: value?.target ?? 1,
  isActive: value?.isActive ?? true,
  rewards: value?.rewards?.length
   ? value.rewards.map((reward) => ({
    type: reward.type,
    amount: reward.amount,
    titleCode: reward.titleCode ?? '',
   }))
   : [{ type: '', amount: 1, titleCode: '' }],
 };
}

export function toAchievementFormValues(value?: AdminAchievementDefinition | null): AdminAchievementFormValues {
 return {
  code: value?.code ?? '',
  titleVi: value?.titleVi ?? '',
  titleEn: value?.titleEn ?? '',
  titleZh: value?.titleZh ?? '',
  descriptionVi: value?.descriptionVi ?? '',
  descriptionEn: value?.descriptionEn ?? '',
  descriptionZh: value?.descriptionZh ?? '',
  icon: value?.icon ?? '',
  grantsTitleCode: value?.grantsTitleCode ?? '',
  isActive: value?.isActive ?? true,
 };
}

export function toTitleFormValues(value?: AdminTitleDefinition | null): AdminTitleFormValues {
 return {
  code: value?.code ?? '',
  nameVi: value?.nameVi ?? '',
  nameEn: value?.nameEn ?? '',
  nameZh: value?.nameZh ?? '',
  descriptionVi: value?.descriptionVi ?? '',
  descriptionEn: value?.descriptionEn ?? '',
  descriptionZh: value?.descriptionZh ?? '',
  rarity: value?.rarity ?? '',
  isActive: value?.isActive ?? true,
 };
}

export function toQuestDefinition(values: AdminQuestFormValues): AdminQuestDefinition {
 return {
  ...values,
  rewards: values.rewards.map((reward) => ({
   type: reward.type.trim(),
   amount: reward.amount,
   titleCode: reward.titleCode.trim() || null,
  })),
 };
}

export function toAchievementDefinition(values: AdminAchievementFormValues): AdminAchievementDefinition {
 return {
  ...values,
  grantsTitleCode: values.grantsTitleCode.trim() || null,
  icon: values.icon.trim() || null,
 };
}

export function toTitleDefinition(values: AdminTitleFormValues): AdminTitleDefinition {
 return values;
}

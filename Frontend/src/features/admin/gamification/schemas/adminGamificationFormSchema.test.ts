import { describe, expect, it } from 'vitest';
import {
 adminAchievementFormSchema,
 adminQuestFormSchema,
 adminTitleFormSchema,
 toAchievementDefinition,
 toAchievementFormValues,
 toQuestDefinition,
 toQuestFormValues,
 toTitleDefinition,
 toTitleFormValues,
} from '@/features/admin/gamification/schemas/adminGamificationFormSchema';

describe('adminGamificationFormSchema mappings', () => {
 it('preserves multilingual quest fields and normalizes optional reward title codes', () => {
  const formValues = toQuestFormValues({
   code: 'daily_sign_in',
   titleVi: 'Diem danh',
   titleEn: 'Check in',
   titleZh: '签到',
   descriptionVi: 'Mo ta',
   descriptionEn: 'Description',
   descriptionZh: '说明',
   questType: 'daily',
   triggerEvent: 'checkin.completed',
   target: 1,
   isActive: true,
   rewards: [{ type: 'title', amount: 1, titleCode: '  ' }],
  });

  const definition = toQuestDefinition(formValues);
  expect(definition.titleZh).toBe('签到');
  expect(definition.descriptionZh).toBe('说明');
  expect(definition.isActive).toBe(true);
  expect(definition.rewards[0]?.titleCode).toBeNull();
 });

 it('keeps admin achievement and title activation fields in the DTO contract', () => {
  const achievement = toAchievementDefinition(toAchievementFormValues({
   code: 'first_spread',
   titleVi: 'Lan dau',
   titleEn: 'First Spread',
   titleZh: '第一次展开',
   descriptionVi: 'Mo ta',
   descriptionEn: 'Description',
   descriptionZh: '说明',
   icon: '  ',
   grantsTitleCode: ' title_master ',
   isActive: false,
  }));
  const title = toTitleDefinition(toTitleFormValues({
   code: 'title_master',
   nameVi: 'Cao thu',
   nameEn: 'Master',
   nameZh: '大师',
   descriptionVi: 'Mo ta',
   descriptionEn: 'Description',
   descriptionZh: '说明',
   rarity: 'epic',
   isActive: true,
  }));

  expect(achievement.icon).toBeNull();
  expect(achievement.grantsTitleCode).toBe('title_master');
  expect(achievement.isActive).toBe(false);
  expect(title.nameZh).toBe('大师');
  expect(title.isActive).toBe(true);
 });

 it('builds safe defaults for empty admin form editors', () => {
  const questDefaults = toQuestFormValues();
  const achievementDefaults = toAchievementFormValues(null);
  const titleDefaults = toTitleFormValues(undefined);

  expect(adminQuestFormSchema.safeParse(questDefaults).success).toBe(false);
  expect(questDefaults).toMatchObject({
   code: '',
   titleZh: '',
   descriptionZh: '',
   target: 1,
   isActive: true,
   rewards: [{ type: '', amount: 1, titleCode: '' }],
  });
  expect(adminAchievementFormSchema.safeParse(achievementDefaults).success).toBe(false);
  expect(achievementDefaults).toMatchObject({
   icon: '',
   grantsTitleCode: '',
   isActive: true,
  });
  expect(adminTitleFormSchema.safeParse(titleDefaults).success).toBe(false);
  expect(titleDefaults).toMatchObject({
   nameZh: '',
   descriptionZh: '',
   rarity: '',
   isActive: true,
  });
 });
});

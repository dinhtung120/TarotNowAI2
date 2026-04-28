'use client';

import type {
 AdminAchievementDefinition,
 AdminGamificationEntityType,
 AdminQuestDefinition,
 AdminTitleDefinition,
} from '@/features/gamification/admin/application/adminGamification.types';
import { AdminAchievementForm } from '@/features/gamification/admin/presentation/AdminAchievementForm';
import { AdminQuestForm } from '@/features/gamification/admin/presentation/AdminQuestForm';
import { AdminTitleForm } from '@/features/gamification/admin/presentation/AdminTitleForm';

interface AdminGamificationFormModalProps {
 entityType: AdminGamificationEntityType | null;
 initialAchievement?: AdminAchievementDefinition | null;
 initialQuest?: AdminQuestDefinition | null;
 initialTitle?: AdminTitleDefinition | null;
 open: boolean;
 onClose: () => void;
 onSubmitAchievement: (value: AdminAchievementDefinition) => Promise<void>;
 onSubmitQuest: (value: AdminQuestDefinition) => Promise<void>;
 onSubmitTitle: (value: AdminTitleDefinition) => Promise<void>;
 submitting: boolean;
}

export function AdminGamificationFormModal(props: AdminGamificationFormModalProps) {
 if (!props.entityType) {
  return null;
 }

 if (props.entityType === 'quest') {
  return <AdminQuestForm initialValue={props.initialQuest} open={props.open} onClose={props.onClose} onSubmit={props.onSubmitQuest} submitting={props.submitting} />;
 }

 if (props.entityType === 'achievement') {
  return <AdminAchievementForm initialValue={props.initialAchievement} open={props.open} onClose={props.onClose} onSubmit={props.onSubmitAchievement} submitting={props.submitting} />;
 }

 return <AdminTitleForm initialValue={props.initialTitle} open={props.open} onClose={props.onClose} onSubmit={props.onSubmitTitle} submitting={props.submitting} />;
}

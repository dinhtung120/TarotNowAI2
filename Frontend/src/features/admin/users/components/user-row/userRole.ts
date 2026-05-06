import type { AdminUsersTranslateFn } from '../types';

export function roleClassName(role: string) {
 if (role === 'admin') return 'bg-[var(--danger)]/10 border-[var(--danger)]/20 text-[var(--danger)]';
 if (role === 'tarot_reader') return 'bg-[var(--info)]/10 border-[var(--info)]/20 text-[var(--info)]';
 return 'tn-surface-strong tn-border text-[var(--text-secondary)]';
}

export function roleLabel(role: string, t: AdminUsersTranslateFn) {
 if (role === 'admin') return t('users.roles.admin');
 if (role === 'tarot_reader') return t('users.roles.tarot_reader');
 if (role === 'user') return t('users.roles.user');
 return role;
}

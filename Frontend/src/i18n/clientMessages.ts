import type { MessageDictionary } from '@/i18n/messages';

function uniqueNamespaces(namespaces: readonly string[]): string[] {
 return [...new Set(namespaces)];
}

export function pickClientMessages(messages: MessageDictionary, namespaces: readonly string[]): MessageDictionary {
 const picked: MessageDictionary = {};
 for (const namespace of namespaces) {
  if (Object.hasOwn(messages, namespace)) {
   picked[namespace] = messages[namespace];
  }
 }

 return picked;
}

export const ROOT_CLIENT_NAMESPACES = uniqueNamespaces([
 'Common',
 'ApiErrors',
 'Navigation',
 'UserNav',
 'LanguageSwitcher',
]);

export const AUTH_CLIENT_NAMESPACES = uniqueNamespaces([
 ...ROOT_CLIENT_NAMESPACES,
 'Auth',
]);

export const SITE_CLIENT_NAMESPACES = uniqueNamespaces([
 ...ROOT_CLIENT_NAMESPACES,
 'Legal',
 'Wallet',
 'Index',
 'Notifications',
 'Chat',
]);

export const USER_CLIENT_NAMESPACES = uniqueNamespaces([
 ...ROOT_CLIENT_NAMESPACES,
 'Auth',
 'Wallet',
 'Notifications',
 'Community',
 'Inventory',
 'Gacha',
 'Collection',
 'Tarot',
 'Profile',
 'Readers',
 'ReaderApply',
 'Chat',
 'ReadingSetup',
 'ReadingSession',
 'AiInterpretation',
 'History',
 'Gamification',
 'Pagination',
 'Index',
]);

export const ADMIN_CLIENT_NAMESPACES = uniqueNamespaces([
 ...ROOT_CLIENT_NAMESPACES,
 'Admin',
]);

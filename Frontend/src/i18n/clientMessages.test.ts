import { describe, expect, it } from 'vitest';
import {
 ROOT_CLIENT_NAMESPACES,
 SITE_CLIENT_NAMESPACES,
 pickClientMessages,
} from './clientMessages';

describe('SITE_CLIENT_NAMESPACES', () => {
 it('includes chat and notifications namespaces for navbar dropdowns', () => {
  expect(SITE_CLIENT_NAMESPACES).toContain('Chat');
  expect(SITE_CLIENT_NAMESPACES).toContain('Notifications');
 });
});

describe('pickClientMessages', () => {
 it('returns only selected namespaces', () => {
  const source = {
   Common: { app_title: 'TarotNow AI' },
   Chat: { dropdown: { title: 'Tin nhan' } },
   Notifications: { title: 'Thong bao' },
   Hidden: { value: 'nope' },
  };

  const picked = pickClientMessages(source, [...ROOT_CLIENT_NAMESPACES, 'Chat', 'Notifications']);
  expect(Object.keys(picked).sort()).toEqual(['Chat', 'Common', 'Notifications']);
  expect(picked.Chat).toEqual(source.Chat);
  expect(picked.Notifications).toEqual(source.Notifications);
  expect(picked.Hidden).toBeUndefined();
 });
});

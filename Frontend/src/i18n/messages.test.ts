import { describe, expect, it } from 'vitest';
import { loadLocaleMessages, mergeMessages } from './messages';

const EXPECTED_TOP_LEVEL_KEYS = [
 'Admin',
 'AiInterpretation',
 'ApiErrors',
 'Auth',
 'Chat',
 'Collection',
 'Common',
 'Footer',
 'History',
 'Index',
 'LanguageSwitcher',
 'Legal',
 'Navigation',
 'Notifications',
 'Pagination',
 'Profile',
 'ReaderApply',
 'Readers',
 'ReadingSession',
 'ReadingSetup',
 'Tarot',
 'UserNav',
 'Wallet',
].sort();

describe('mergeMessages', () => {
 it('deep merges nested dictionaries', () => {
  const base = {
   a: 1,
   nested: {
    x: 'base',
    y: 'stay',
   },
  };

  const override = {
   nested: {
    x: 'override',
    z: 'new',
   },
   b: 2,
  };

  expect(mergeMessages(base, override)).toEqual({
   a: 1,
   nested: {
    x: 'override',
    y: 'stay',
    z: 'new',
   },
   b: 2,
  });
 });
});

describe('loadLocaleMessages', () => {
 for (const locale of ['en', 'vi', 'zh'] as const) {
  it(`loads full modular dictionary for ${locale}`, async () => {
   const messages = await loadLocaleMessages(locale);

   expect(Object.keys(messages).sort()).toEqual(EXPECTED_TOP_LEVEL_KEYS);

   const profile = messages.Profile as Record<string, unknown>;
   const validation = profile.validation as Record<string, unknown>;
   expect(validation.display_name_min).toBeTypeOf('string');
  });
 }
});

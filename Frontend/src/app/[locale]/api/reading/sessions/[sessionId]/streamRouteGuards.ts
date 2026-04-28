const SESSION_ID_PATTERN = /^[A-Za-z0-9-]+$/;
const SESSION_ID_MAX_LENGTH = 64;
const ALLOWED_LANGUAGES = new Set(['vi', 'en', 'zh']);
const FOLLOWUP_MAX_LENGTH = 2000;

export function getSafeStreamLanguage(rawLanguage: string | null): string {
 if (!rawLanguage) return 'en';
 const language = rawLanguage.trim().toLowerCase();
 if (!language) return 'en';
 if (ALLOWED_LANGUAGES.has(language)) return language;
 const baseLanguage = language.split('-')[0];
 return ALLOWED_LANGUAGES.has(baseLanguage) ? baseLanguage : 'en';
}

export function getSafeFollowupQuestion(rawQuestion: string | null): string | undefined {
 if (!rawQuestion) return undefined;
 const question = rawQuestion.replace(/\u0000/g, '').replace(/\r/g, '').trim();
 return question ? question.slice(0, FOLLOWUP_MAX_LENGTH) : undefined;
}

export function isValidStreamSessionId(sessionId: string): boolean {
 return SESSION_ID_PATTERN.test(sessionId) && sessionId.length <= SESSION_ID_MAX_LENGTH;
}

import { readdir, readFile } from 'node:fs/promises';
import type { Dirent } from 'node:fs';
import path from 'node:path';
import { routing } from './routing';

type AppLocale = (typeof routing.locales)[number];
type MessageDictionary = Record<string, unknown>;

const MODULAR_MESSAGES_ROOT = path.join(process.cwd(), 'messages');

const isRecord = (value: unknown): value is MessageDictionary =>
 typeof value === 'object' && value !== null && !Array.isArray(value);

const isErrnoException = (error: unknown): error is NodeJS.ErrnoException =>
 typeof error === 'object' &&
 error !== null &&
 'code' in error;

export const mergeMessages = (
 baseMessages: MessageDictionary,
 overrideMessages: MessageDictionary
): MessageDictionary => {
 const merged: MessageDictionary = { ...baseMessages };

 for (const [key, value] of Object.entries(overrideMessages)) {
  const currentValue = merged[key];
  merged[key] = isRecord(currentValue) && isRecord(value)
   ? mergeMessages(currentValue, value)
   : value;
 }

 return merged;
};

const parseMessageDictionary = (content: string, filePath: string): MessageDictionary => {
 const parsed = JSON.parse(content) as unknown;
 if (!isRecord(parsed)) {
  throw new Error(`Invalid message dictionary at ${filePath}. Expected an object.`);
 }

 return parsed;
};

const listJsonFilesRecursively = async (directoryPath: string): Promise<string[]> => {
 let entries: Dirent[];

 try {
  entries = await readdir(directoryPath, { withFileTypes: true });
 } catch (error) {
  if (isErrnoException(error) && error.code === 'ENOENT') {
   return [];
  }

  throw error;
 }

 const files = await Promise.all(entries.map(async (entry) => {
  const entryPath = path.join(directoryPath, entry.name);

  if (entry.isDirectory()) {
   return listJsonFilesRecursively(entryPath);
  }

  if (entry.isFile() && entry.name.endsWith('.json')) {
   return [entryPath];
  }

  return [];
 }));

 return files.flat().sort((a, b) => a.localeCompare(b));
};

const loadModularMessages = async (locale: AppLocale): Promise<MessageDictionary> => {
 const localeDirectory = path.join(MODULAR_MESSAGES_ROOT, locale);
 const messageFiles = await listJsonFilesRecursively(localeDirectory);

 if (messageFiles.length === 0) {
  throw new Error(`Missing modular i18n messages for locale "${locale}" in ${localeDirectory}`);
 }

 let mergedMessages: MessageDictionary = {};

 for (const messageFile of messageFiles) {
  const content = await readFile(messageFile, 'utf8');
  const dictionary = parseMessageDictionary(content, messageFile);
  mergedMessages = mergeMessages(mergedMessages, dictionary);
 }

 return mergedMessages;
};

export const loadLocaleMessages = loadModularMessages;

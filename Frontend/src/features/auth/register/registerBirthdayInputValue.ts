export function formatIsoToDisplay(value?: string): string {
 if (!value || !value.includes('-')) {
  return '';
 }

 const [year, month, day] = value.split('-');
 return `${day}/${month}/${year}`;
}

export function formatBirthdayDraft(rawValue: string): string {
 const digits = rawValue.replace(/\D/g, '').slice(0, 8);
 if (digits.length <= 2) return digits;
 if (digits.length <= 4) return `${digits.slice(0, 2)}/${digits.slice(2, 4)}`;
 return `${digits.slice(0, 2)}/${digits.slice(2, 4)}/${digits.slice(4, 8)}`;
}

export function parseDisplayBirthday(value: string): string {
 const digits = value.replace(/\D/g, '');
 if (digits.length !== 8) {
  return '';
 }

 const day = digits.slice(0, 2);
 const month = digits.slice(2, 4);
 const year = digits.slice(4, 8);
 return `${year}-${month}-${day}`;
}

export function normalizeParticipantId(value: string | null | undefined): string {
  return (value ?? '').trim().toLowerCase();
}

export function isSameParticipantId(
  left: string | null | undefined,
  right: string | null | undefined,
): boolean {
  const normalizedLeft = normalizeParticipantId(left);
  const normalizedRight = normalizeParticipantId(right);
  return normalizedLeft.length > 0 && normalizedLeft === normalizedRight;
}

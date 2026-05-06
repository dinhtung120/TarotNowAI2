const SPREAD_LABELS: Record<string, string> = {
 Daily1Card: 'spread_daily',
 daily_1: 'spread_daily',
 PastPresentFuture: 'spread_3',
 spread_3: 'spread_3',
 spread_5: 'spread_5',
 spread_10: 'spread_10',
};

export function resolveSpreadName(spreadType: string, getLabel: (labelKey: string) => string) {
 const labelKey = SPREAD_LABELS[spreadType];
 return labelKey ? getLabel(labelKey) : spreadType;
}

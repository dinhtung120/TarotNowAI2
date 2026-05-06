interface GachaLocalizedText {
 nameVi: string;
 nameEn: string;
 nameZh: string;
}

interface GachaLocalizedDescription {
 descriptionVi: string;
 descriptionEn: string;
 descriptionZh: string;
}

export function localizeGachaName(locale: string, value: GachaLocalizedText): string {
 if (locale === 'en') return value.nameEn;
 if (locale === 'zh') return value.nameZh;
 return value.nameVi;
}

export function localizeGachaDescription(locale: string, value: GachaLocalizedDescription): string {
 if (locale === 'en') return value.descriptionEn;
 if (locale === 'zh') return value.descriptionZh;
 return value.descriptionVi;
}

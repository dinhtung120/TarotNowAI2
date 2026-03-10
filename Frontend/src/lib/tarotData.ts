// Tarot Card Metadata for MVP
export const TAROT_DECK = [
    { id: 0, name: "The Fool", suit: "Major Arcana", meaning: "New beginnings, optimism, trust in life, spontaneity." },
    { id: 1, name: "The Magician", suit: "Major Arcana", meaning: "Action, the power to manifest, resourcefulness." },
    { id: 2, name: "The High Priestess", suit: "Major Arcana", meaning: "Inaction, going within, intuition, mystery." },
    { id: 3, name: "The Empress", suit: "Major Arcana", meaning: "Abundance, nurturing, fertility, life in bloom!" },
    { id: 4, name: "The Emperor", suit: "Major Arcana", meaning: "Structure, stability, rules and power." },
    { id: 5, name: "The Hierophant", suit: "Major Arcana", meaning: "Institutions, tradition, society and its rules." },
    { id: 6, name: "The Lovers", suit: "Major Arcana", meaning: "Sexuality, passion, choice, uniting." },
    { id: 7, name: "The Chariot", suit: "Major Arcana", meaning: "Movement, progress, integration, victory." },
    { id: 8, name: "Strength", suit: "Major Arcana", meaning: "Courage, subtle power, integration of animal self." },
    { id: 9, name: "The Hermit", suit: "Major Arcana", meaning: "Meditation, solitude, consciousness." },
    { id: 10, name: "Wheel of Fortune", suit: "Major Arcana", meaning: "Cycles, change, ups and downs, destiny." },
    { id: 11, name: "Justice", suit: "Major Arcana", meaning: "Fairness, equality, balance, legal matters." },
    { id: 12, name: "The Hanged Man", suit: "Major Arcana", meaning: "Surrender, new perspective, enlightenment." },
    { id: 13, name: "Death", suit: "Major Arcana", meaning: "The end of something, change, the impermeability of all things." },
    { id: 14, name: "Temperance", suit: "Major Arcana", meaning: "Balance, moderation, being sensible." },
    { id: 15, name: "The Devil", suit: "Major Arcana", meaning: "Destructive patterns, addiction, giving away your power." },
    { id: 16, name: "The Tower", suit: "Major Arcana", meaning: "Collapse of stable structures, release, sudden insight." },
    { id: 17, name: "The Star", suit: "Major Arcana", meaning: "Hope, calm, a good omen!" },
    { id: 18, name: "The Moon", suit: "Major Arcana", meaning: "Mystery, the subconscious, dreams, anxiety." },
    { id: 19, name: "The Sun", suit: "Major Arcana", meaning: "Success, happiness, all will be well." },
    { id: 20, name: "Judgement", suit: "Major Arcana", meaning: "Rebirth, a new phase, inner calling." },
    { id: 21, name: "The World", suit: "Major Arcana", meaning: "Completion, wholeness, attainment, celebration of life." },
    // Fill Minor Arcana placeholders to reach 78 cards
    ...Array.from({ length: 56 }).map((_, i) => ({
        id: i + 22,
        name: `Minor Arcana Card ${i + 1}`,
        suit: "Minor Arcana",
        meaning: "A minor arcana card aspect."
    }))
];

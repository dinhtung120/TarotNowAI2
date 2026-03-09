// TarotWeb MongoDB Init – Collections & Indexes
// Chạy: mongosh <connection_string> < init.js
// Mục đích: Tạo collections và indexes theo schema. Xem schema.md cho mô tả chi tiết.
// Lưu ý: createCollection/createIndex idempotent; nếu chạy lại có thể báo index đã tồn tại – bỏ qua.

(function () {
  "use strict";

  // 1. Content & Inventory – 78 lá bài, túi bài, Ascension stories
  try {
    db.createCollection("cards_catalog", {
      validator: {
        $jsonSchema: {
          bsonType: "object",
          properties: {
            name: {
              bsonType: "object",
              required: ["vi", "en", "zh"],
              properties: {
                vi: { bsonType: "string" },
                en: { bsonType: "string" },
                zh: { bsonType: "string" }
              }
            }
          }
        }
      },
      validationLevel: "moderate",
      validationAction: "warn"
    });
  } catch (e) {
    if (e.code !== 48) throw e;
  }
  db.cards_catalog.createIndex({ code: 1 }, { unique: true });
  db.cards_catalog.createIndex({ "name.vi": 1 }, { unique: true });
  db.cards_catalog.createIndex({ "name.en": 1 }, { unique: true });
  db.cards_catalog.createIndex({ "name.zh": 1 }, { unique: true });

  db.createCollection("user_collections");
  db.user_collections.createIndex({ user_id: 1, card_id: 1 }, { unique: true });
  db.user_collections.createIndex({ user_id: 1, level: -1 });
  db.user_collections.createIndex({ is_deleted: 1 });

  db.createCollection("card_stories");
  db.card_stories.createIndex({ user_id: 1, card_id: 1, level_trigger: 1 }, { unique: true });
  db.card_stories.createIndex({ card_id: 1, level_trigger: 1 });
  db.card_stories.createIndex({ is_deleted: 1 }); // Consistency với các collection khác khi exclude deleted

  // 2. Reader – Hồ sơ Reader, đơn xin Reader
  try {
    db.createCollection("reader_profiles", {
      validator: {
        $jsonSchema: {
          bsonType: "object",
          properties: {
            status: { enum: ["online", "offline", "accepting_questions"] }
          }
        }
      },
      validationLevel: "moderate",
      validationAction: "warn"
    });
  } catch (e) {
    if (e.code !== 48) throw e;
  }
  db.reader_profiles.createIndex({ user_id: 1 }, { unique: true });
  db.reader_profiles.createIndex({ status: 1, updated_at: -1 });
  db.reader_profiles.createIndex({ status: 1, "stats.avg_rating": -1 });  // Listing theo rating
  db.reader_profiles.createIndex({ is_deleted: 1 });

  db.createCollection("reader_requests");
  db.reader_requests.createIndex({ user_id: 1, created_at: -1 });
  db.reader_requests.createIndex({ status: 1, created_at: -1 });
  db.reader_requests.createIndex({ is_deleted: 1 });

  // 3. Readings – Phiên xem bài, friend chain (validator: spread_type enum, drawn_cards max 10)
  try {
    db.createCollection("reading_sessions", {
      validator: {
        $jsonSchema: {
          bsonType: "object",
          required: ["user_id", "spread_type", "created_at"],
          properties: {
            spread_type: { enum: ["daily_1", "spread_3", "spread_5", "spread_10"] },
            drawn_cards: { bsonType: "array", maxItems: 10 }
          }
        }
      },
      validationLevel: "moderate",
      validationAction: "warn"
    });
  } catch (e) {
    if (e.code !== 48) throw e;  // 48 = namespace exists, bỏ qua
  }
  db.reading_sessions.createIndex({ user_id: 1, created_at: -1 });
  db.reading_sessions.createIndex({ ai_status: 1, created_at: -1 });
  db.reading_sessions.createIndex({ user_id: 1, spread_type: 1, created_at: -1 }); // Check daily 1 card limit
  db.reading_sessions.createIndex({ is_deleted: 1 });

  db.createCollection("reading_chains");
  db.reading_chains.createIndex({ host_user_id: 1, guest_user_id: 1, business_date: 1 }, { unique: true });  // L3 fix: cho phép mời lại cùng cặp per day
  db.reading_chains.createIndex({ is_deleted: 1 });  // L4 fix: consistency với các collection có soft delete

  // 4. Chat – conversations + messages, tham chiếu finance_session_ref
  try {
    db.createCollection("conversations", {
      validator: {
        $jsonSchema: {
          bsonType: "object",
          properties: {
            status: { enum: ["pending", "active", "completed", "cancelled", "disputed"] }
          }
        }
      },
      validationLevel: "moderate",
      validationAction: "warn"
    });
  } catch (e) {
    if (e.code !== 48) throw e;
  }
  db.conversations.createIndex({ user_id: 1, status: 1, updated_at: -1 });
  db.conversations.createIndex({ reader_id: 1, status: 1, updated_at: -1 });
  db.conversations.createIndex({ finance_session_ref: 1 }); // Lookup từ PostgreSQL chat_finance_sessions
  db.conversations.createIndex({ is_deleted: 1 });

  // M-1 fix: Thêm validator cho chat_messages.type
  try {
    db.createCollection("chat_messages", {
      validator: {
        $jsonSchema: {
          bsonType: "object",
          properties: {
            type: { enum: ["text", "system", "card_share", "payment_offer", "payment_accept", "payment_reject", "system_refund", "system_release", "system_dispute"] }
          }
        }
      },
      validationLevel: "moderate",
      validationAction: "warn"
    });
  } catch (e) {
    if (e.code !== 48) throw e;
  }
  db.chat_messages.createIndex({ conversation_id: 1, created_at: -1 });
  db.chat_messages.createIndex({ sender_id: 1, created_at: -1 });
  db.chat_messages.createIndex({ is_deleted: 1 });  // Consistency với các collection khác có soft delete

  // 5. Social – Reviews, reports, referrals
  db.createCollection("reviews");
  db.reviews.createIndex({ "target.type": 1, "target.id": 1, created_at: -1 });
  db.reviews.createIndex({ author_id: 1, created_at: -1 });  // V1 fix: hỗ trợ query "reviews user đã viết"
  db.reviews.createIndex({ is_deleted: 1 });  // L5 fix: consistency với các collection có soft delete

  db.createCollection("reports");
  db.reports.createIndex({ status: 1, created_at: -1 });
  db.reports.createIndex({ "target.type": 1, "target.id": 1, created_at: -1 });
  db.reports.createIndex({ is_deleted: 1 });

  db.createCollection("referrals");
  db.referrals.createIndex({ inviter_id: 1, invited_user_id: 1 }, { unique: true });
  db.referrals.createIndex({ inviter_id: 1, created_at: -1 });

  // 6. Gamification – Quests, achievements, titles
  db.createCollection("quests");
  db.quests.createIndex({ code: 1 }, { unique: true });
  db.quests.createIndex({ type: 1, is_active: 1 });

  db.createCollection("quest_progress");
  db.quest_progress.createIndex({ user_id: 1, quest_code: 1, period_key: 1 }, { unique: true });
  db.quest_progress.createIndex({ quest_code: 1, period_key: 1, is_completed: 1 });

  db.createCollection("achievements");
  db.achievements.createIndex({ code: 1 }, { unique: true });

  db.createCollection("user_achievements");
  db.user_achievements.createIndex({ user_id: 1, achievement_code: 1 }, { unique: true });

  db.createCollection("titles");
  db.titles.createIndex({ code: 1 }, { unique: true });

  db.createCollection("user_titles");
  db.user_titles.createIndex({ user_id: 1, title_ref: 1 }, { unique: true });

  // 7. Events & Notifications – events_config, notifications (TTL 30d), daily_checkins
  db.createCollection("events_config");
  db.events_config.createIndex({ code: 1 }, { unique: true });
  db.events_config.createIndex({ is_active: 1, available_from: 1, available_until: 1 });

  db.createCollection("notifications");
  db.notifications.createIndex({ user_id: 1, is_read: 1, created_at: -1 });
  db.notifications.createIndex({ created_at: 1 }, { expireAfterSeconds: 2592000 }); // 30 days

  db.createCollection("daily_checkins");
  db.daily_checkins.createIndex({ user_id: 1, business_date: 1 }, { unique: true });

  // 8. Logs – ai_provider_logs (TTL 90d), admin_logs, gacha_logs (TTL 180d), leaderboard_snapshots
  // L4 fix: đổi tên grok_logs → ai_provider_logs (hỗ trợ nhiều provider)
  db.createCollection("ai_provider_logs");
  db.ai_provider_logs.createIndex({ user_id: 1, created_at: -1 });
  db.ai_provider_logs.createIndex({ status: 1, created_at: -1 });  // C5 fix: index cho alert/debug theo status
  db.ai_provider_logs.createIndex({ created_at: 1 }, { expireAfterSeconds: 7776000 }); // 90 days

  db.createCollection("admin_logs");
  db.admin_logs.createIndex({ admin_id: 1, created_at: -1 });
  db.admin_logs.createIndex({ action: 1, created_at: -1 });

  db.createCollection("gacha_logs");
  db.gacha_logs.createIndex({ user_id: 1, created_at: -1 });
  db.gacha_logs.createIndex({ created_at: 1 }, { expireAfterSeconds: 15552000 }); // 180 days

  try {
    db.createCollection("leaderboard_snapshots", {
      validator: {
        $jsonSchema: {
          bsonType: "object",
          properties: {
            type: { enum: ["daily_rank_score", "monthly_rank_score", "lifetime_score", "achievement_points", "diamond_spend", "top_readers"] }
          }
        }
      },
      validationLevel: "moderate",
      validationAction: "warn"
    });
  } catch (e) {
    if (e.code !== 48) throw e;
  }
  db.leaderboard_snapshots.createIndex({ period_key: 1, type: 1 }, { unique: true });

  // 9. Phase 4 – Community (posts, reactions), call_sessions (voice/video)
  db.createCollection("community_posts");
  db.community_posts.createIndex({ created_at: -1 });
  db.community_posts.createIndex({ author_id: 1, created_at: -1 });
  db.community_posts.createIndex({ visibility: 1, created_at: -1 }); // Filter public posts
  db.community_posts.createIndex({ is_deleted: 1 });  // L2 fix: consistency với các collection có soft delete

  db.createCollection("community_reactions");
  db.community_reactions.createIndex({ post_id: 1, user_id: 1, type: 1 }, { unique: true });
  db.community_reactions.createIndex({ user_id: 1, created_at: -1 });

  try {
    db.createCollection("call_sessions", {
      validator: {
        $jsonSchema: {
          bsonType: "object",
          properties: {
            status: { enum: ["requested", "accepted", "rejected", "ended"] }
          }
        }
      },
      validationLevel: "moderate",
      validationAction: "warn"
    });
  } catch (e) {
    if (e.code !== 48) throw e;
  }
  db.call_sessions.createIndex({ conversation_id: 1, created_at: -1 });
  db.call_sessions.createIndex({ status: 1, conversation_id: 1 });  // Filter active calls

  // H5 fix: share_claims collection cho anti-abuse pipeline
  db.createCollection("share_claims");
  db.share_claims.createIndex({ user_id: 1, created_at: -1 });
  db.share_claims.createIndex({ status: 1, created_at: -1 });
  db.share_claims.createIndex({ user_id: 1, platform: 1, created_at: -1 });  // Velocity check per user per platform

  print("TarotWeb MongoDB init completed.");
  // Sharding (Tier L): chat_messages có thể shard key {conversation_id: "hashed"}. Cần enableSharding trước.

})();

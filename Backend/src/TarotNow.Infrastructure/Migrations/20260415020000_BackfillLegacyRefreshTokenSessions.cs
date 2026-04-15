using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using TarotNow.Infrastructure.Persistence;

#nullable disable

namespace TarotNow.Infrastructure.Migrations;

/// <summary>
/// Backfill auth_sessions cho refresh tokens legacy chưa có session_id để hoàn tất migration zero-downtime.
/// </summary>
[DbContext(typeof(ApplicationDbContext))]
[Migration("20260415020000_BackfillLegacyRefreshTokenSessions")]
public partial class BackfillLegacyRefreshTokenSessions : Migration
{
    private const string EmptySessionId = "00000000-0000-0000-0000-000000000000";

    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(
            $"""
             WITH legacy_tokens AS (
                 SELECT
                     rt.user_id,
                     COALESCE(NULLIF(rt.created_device_id, ''), 'legacy-unknown') AS device_id,
                     COALESCE(NULLIF(rt.created_user_agent_hash, ''), 'unknown') AS user_agent_hash,
                     COALESCE(NULLIF(rt.created_by_ip, ''), 'unknown') AS last_ip_hash,
                     MIN(rt.created_at) AS first_seen_at
                 FROM refresh_tokens rt
                 WHERE rt.session_id = '{EmptySessionId}'::uuid
                 GROUP BY rt.user_id, COALESCE(NULLIF(rt.created_device_id, ''), 'legacy-unknown'), COALESCE(NULLIF(rt.created_user_agent_hash, ''), 'unknown'), COALESCE(NULLIF(rt.created_by_ip, ''), 'unknown')
             ),
             to_insert AS (
                 SELECT
                     md5('legacy:' || lt.user_id::text || ':' || lt.device_id)::uuid AS id,
                     lt.user_id,
                     lt.device_id,
                     lt.user_agent_hash,
                     left(lt.last_ip_hash, 128) AS last_ip_hash,
                     lt.first_seen_at
                 FROM legacy_tokens lt
                 WHERE NOT EXISTS (
                     SELECT 1
                     FROM auth_sessions existing
                     WHERE existing.user_id = lt.user_id
                       AND existing.device_id = lt.device_id
                       AND existing.revoked_at_utc IS NULL
                 )
             )
             INSERT INTO auth_sessions (
                 id,
                 user_id,
                 device_id,
                 user_agent_hash,
                 last_ip_hash,
                 created_at_utc,
                 last_seen_at_utc,
                 revoked_at_utc
             )
             SELECT
                 ti.id,
                 ti.user_id,
                 ti.device_id,
                 left(ti.user_agent_hash, 128),
                 ti.last_ip_hash,
                 ti.first_seen_at,
                 ti.first_seen_at,
                 NULL
             FROM to_insert ti
             ON CONFLICT (id) DO NOTHING;
             """);

        migrationBuilder.Sql(
            $"""
             UPDATE refresh_tokens rt
             SET session_id = s.id
             FROM auth_sessions s
             WHERE rt.session_id = '{EmptySessionId}'::uuid
               AND s.user_id = rt.user_id
               AND s.device_id = COALESCE(NULLIF(rt.created_device_id, ''), 'legacy-unknown')
               AND s.revoked_at_utc IS NULL;
             """);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // Additive migration: không rollback dữ liệu đã backfill để tránh mất liên kết session.
    }
}

# TarotNow Benchmark Analysis

- Run time (UTC): 2026-05-08T18:12:54.962Z
- Base: https://www.tarotnow.xyz/vi
- Benchmark mode: full-matrix
- Matrix: Chromium desktop + mobile, logged-out + logged-in-admin + logged-in-reader

## Scenario Summary
| Scenario | Viewport | Pages | Avg requests/page | Avg nav (ms) | Pending | Doc Reloads | Handshake Redirects | Session API Calls | Failed Requests | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 24.9 | 2872 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | desktop | 43 | 29.6 | 3000 | 2 | 0 | 1 | 9 | 3 | yes |
| logged-in-reader | desktop | 33 | 30.9 | 3043 | 1 | 0 | 2 | 13 | 0 | yes |
| logged-out | mobile | 9 | 25.0 | 2762 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | mobile | 43 | 30.1 | 2960 | 1 | 0 | 1 | 11 | 0 | yes |
| logged-in-reader | mobile | 33 | 32.1 | 3048 | 1 | 0 | 2 | 22 | 0 | yes |

## Route-Family Summary
| Scenario | Viewport | Route Family | Pages | Avg Requests/Page | Avg Navigation (ms) | Pending |
| --- | --- | --- | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | 10 | 29.3 | 2895 | 0 |
| logged-in-admin | desktop | chat | 1 | 28.0 | 2742 | 0 |
| logged-in-admin | desktop | collection | 1 | 31.0 | 6703 | 0 |
| logged-in-admin | desktop | community | 1 | 35.0 | 3990 | 0 |
| logged-in-admin | desktop | gacha | 2 | 30.0 | 2871 | 0 |
| logged-in-admin | desktop | gamification | 1 | 36.0 | 3426 | 1 |
| logged-in-admin | desktop | home | 1 | 26.0 | 2790 | 0 |
| logged-in-admin | desktop | inventory | 1 | 35.0 | 3135 | 0 |
| logged-in-admin | desktop | leaderboard | 1 | 32.0 | 3269 | 0 |
| logged-in-admin | desktop | legal | 3 | 25.0 | 2752 | 0 |
| logged-in-admin | desktop | notifications | 1 | 28.0 | 2754 | 0 |
| logged-in-admin | desktop | profile | 3 | 29.0 | 2853 | 0 |
| logged-in-admin | desktop | reader | 1 | 28.0 | 2772 | 0 |
| logged-in-admin | desktop | readers | 7 | 28.0 | 2761 | 0 |
| logged-in-admin | desktop | reading | 5 | 28.4 | 2866 | 0 |
| logged-in-admin | desktop | wallet | 4 | 35.0 | 3066 | 1 |
| logged-in-admin | mobile | admin | 10 | 29.3 | 2876 | 0 |
| logged-in-admin | mobile | chat | 1 | 28.0 | 3009 | 0 |
| logged-in-admin | mobile | collection | 1 | 29.0 | 5627 | 0 |
| logged-in-admin | mobile | community | 1 | 30.0 | 3681 | 0 |
| logged-in-admin | mobile | gacha | 2 | 33.5 | 2943 | 0 |
| logged-in-admin | mobile | gamification | 1 | 29.0 | 2840 | 1 |
| logged-in-admin | mobile | home | 1 | 26.0 | 2749 | 0 |
| logged-in-admin | mobile | inventory | 1 | 33.0 | 2841 | 0 |
| logged-in-admin | mobile | leaderboard | 1 | 32.0 | 3001 | 0 |
| logged-in-admin | mobile | legal | 3 | 25.0 | 2757 | 0 |
| logged-in-admin | mobile | notifications | 1 | 28.0 | 2695 | 0 |
| logged-in-admin | mobile | profile | 3 | 29.0 | 2881 | 0 |
| logged-in-admin | mobile | reader | 1 | 28.0 | 2759 | 0 |
| logged-in-admin | mobile | readers | 7 | 28.6 | 2823 | 0 |
| logged-in-admin | mobile | reading | 5 | 29.4 | 2835 | 0 |
| logged-in-admin | mobile | wallet | 4 | 40.8 | 3148 | 0 |
| logged-in-reader | desktop | chat | 1 | 28.0 | 2828 | 0 |
| logged-in-reader | desktop | collection | 1 | 29.0 | 6366 | 0 |
| logged-in-reader | desktop | community | 1 | 30.0 | 3676 | 0 |
| logged-in-reader | desktop | gacha | 2 | 32.0 | 2907 | 0 |
| logged-in-reader | desktop | gamification | 1 | 29.0 | 2874 | 1 |
| logged-in-reader | desktop | home | 1 | 26.0 | 2775 | 0 |
| logged-in-reader | desktop | inventory | 1 | 30.0 | 2863 | 0 |
| logged-in-reader | desktop | leaderboard | 1 | 30.0 | 2881 | 0 |
| logged-in-reader | desktop | legal | 3 | 25.0 | 2723 | 0 |
| logged-in-reader | desktop | notifications | 1 | 29.0 | 2887 | 0 |
| logged-in-reader | desktop | profile | 3 | 29.7 | 2861 | 0 |
| logged-in-reader | desktop | reader | 1 | 28.0 | 2789 | 0 |
| logged-in-reader | desktop | readers | 7 | 28.6 | 2844 | 0 |
| logged-in-reader | desktop | reading | 5 | 28.2 | 2806 | 0 |
| logged-in-reader | desktop | wallet | 4 | 48.3 | 3497 | 0 |
| logged-in-reader | mobile | chat | 1 | 28.0 | 2825 | 0 |
| logged-in-reader | mobile | collection | 1 | 29.0 | 5518 | 0 |
| logged-in-reader | mobile | community | 1 | 30.0 | 3483 | 0 |
| logged-in-reader | mobile | gacha | 2 | 33.0 | 2887 | 0 |
| logged-in-reader | mobile | gamification | 1 | 29.0 | 2867 | 1 |
| logged-in-reader | mobile | home | 1 | 26.0 | 2706 | 0 |
| logged-in-reader | mobile | inventory | 1 | 33.0 | 2849 | 0 |
| logged-in-reader | mobile | leaderboard | 1 | 30.0 | 2829 | 0 |
| logged-in-reader | mobile | legal | 3 | 25.0 | 2963 | 0 |
| logged-in-reader | mobile | notifications | 1 | 31.0 | 2926 | 0 |
| logged-in-reader | mobile | profile | 3 | 29.7 | 2881 | 0 |
| logged-in-reader | mobile | reader | 1 | 28.0 | 2767 | 0 |
| logged-in-reader | mobile | readers | 7 | 29.6 | 2848 | 0 |
| logged-in-reader | mobile | reading | 5 | 32.2 | 2934 | 0 |
| logged-in-reader | mobile | wallet | 4 | 49.0 | 3477 | 0 |
| logged-out | desktop | auth | 5 | 24.0 | 2790 | 0 |
| logged-out | desktop | home | 1 | 29.0 | 3695 | 0 |
| logged-out | desktop | legal | 3 | 25.0 | 2732 | 0 |
| logged-out | mobile | auth | 5 | 24.0 | 2687 | 0 |
| logged-out | mobile | home | 1 | 30.0 | 3207 | 0 |
| logged-out | mobile | legal | 3 | 25.0 | 2738 | 0 |

## Key Findings
1. Critical: 9 page(s) vượt 35 requests.
2. High: 142 page(s) vượt 25 requests.
3. High: 38 request vượt 800ms.
4. Medium: 232 request trong dải 400-800ms.
5. High: phát hiện 6 handshake redirect(s), cần kiểm tra vòng lặp auth/session.
6. Collection-focus: 0 image request(s) >800ms trên 0 image request(s).

## Top Slow Pages
| Scenario | Viewport | Feature | Route | Navigate (ms) | Request count | LCP (ms) | TBT (ms) | CLS |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | 6703 | 31 | 992 | 0.0 | 0.0042 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 6366 | 29 | 884 | 0.0 | 0.0040 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 5627 | 29 | 700 | 0.0 | 0.0000 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 5518 | 29 | 544 | 18.0 | 0.0000 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 4531 | 80 | 1160 | 139.0 | 0.0033 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | 4501 | 80 | 820 | 0.0 | 0.0055 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 4231 | 80 | 876 | 0.0 | 0.0000 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | 3990 | 35 | 1984 | 0.0 | 0.0041 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 3880 | 56 | 1572 | 11.0 | 0.0000 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet | 3768 | 56 | 1524 | 24.0 | 0.0000 |

## High Slow Requests (> 800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/gamification | GET | 200 | 1367 | 334 | https://www.tarotnow.xyz/vi/gamification |
| logged-in-reader | mobile | auth-public | /vi/legal/ai-disclaimer | GET | 200 | 1276 | 313 | https://www.tarotnow.xyz/vi/legal/ai-disclaimer |
| logged-out | mobile | auth-public | /vi | GET | 200 | 1030 | 572 | https://www.tarotnow.xyz/vi |
| logged-out | desktop | auth-public | /vi | GET | 200 | 1019 | 545 | https://www.tarotnow.xyz/_next/static/chunks/0.ekkbyazw_22.js |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | GET | 200 | 1015 | 562 | https://www.tarotnow.xyz/vi/inventory |
| logged-out | desktop | auth-public | /vi | GET | 200 | 992 | 342 | https://www.tarotnow.xyz/vi |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 969 | 503 | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1920&q=75 |
| logged-in-admin | mobile | reader-chat | /vi/chat | GET | 200 | 924 | 494 | https://www.tarotnow.xyz/vi/chat |
| logged-in-reader | mobile | auth-public | /vi/legal/ai-disclaimer | GET | 200 | 918 | 119 | https://www.tarotnow.xyz/_next/static/chunks/0.ekkbyazw_22.js |
| logged-out | desktop | auth-public | /vi | GET | 200 | 892 | 562 | https://www.tarotnow.xyz/_next/static/chunks/0o3-.av4h_47m.js |
| logged-in-admin | desktop | reading | /vi/reading/session/2fdb7785-2e4b-405a-aaf6-1bf1580e54f0 | GET | 200 | 883 | 331 | https://www.tarotnow.xyz/vi/reading/session/2fdb7785-2e4b-405a-aaf6-1bf1580e54f0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | GET | 200 | 879 | 861 | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Fpower_booster_50_20260416_165453.avif&w=48&q=75 |
| logged-in-reader | mobile | reading | /vi/reading/session/669dd000-19ea-49a3-8d21-b3530ac0bca9 | GET | 200 | 867 | 310 | https://www.tarotnow.xyz/vi/reading/session/669dd000-19ea-49a3-8d21-b3530ac0bca9 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 863 | 358 | https://www.tarotnow.xyz/vi/notifications |
| logged-in-admin | mobile | admin | /vi/admin/readings | GET | 200 | 860 | 317 | https://www.tarotnow.xyz/vi/admin/readings |

## Medium Slow Requests (400-800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- |
| logged-in-reader | mobile | reading | /vi/reading | GET | 200 | 800 | 333 | https://www.tarotnow.xyz/vi/reading |
| logged-in-reader | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 797 | 318 | https://www.tarotnow.xyz/vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | GET | 200 | 796 | 779 | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Fexp_booster_50_20260416_165452.avif&w=48&q=75 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | GET | 200 | 796 | 329 | https://www.tarotnow.xyz/vi/gacha |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 796 | 325 | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 795 | 424 | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 793 | 326 | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 793 | 356 | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/inventory | GET | 200 | 791 | 312 | https://www.tarotnow.xyz/vi/inventory |
| logged-out | desktop | auth-public | /vi/forgot-password | GET | 200 | 789 | 332 | https://www.tarotnow.xyz/vi/forgot-password |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | GET | 200 | 789 | 323 | https://www.tarotnow.xyz/vi/profile |
| logged-in-admin | desktop | admin | /vi/admin/disputes | GET | 200 | 788 | 333 | https://www.tarotnow.xyz/vi/admin/disputes |
| logged-in-admin | desktop | admin | /vi/admin | GET | 200 | 787 | 565 | https://www.tarotnow.xyz/_next/static/chunks/0-8rlz0vpjwdd.js |
| logged-in-admin | mobile | auth-public | /vi/legal/ai-disclaimer | GET | 200 | 785 | 361 | https://www.tarotnow.xyz/vi/legal/ai-disclaimer |
| logged-in-admin | desktop | reading | /vi/reading | GET | 200 | 782 | 320 | https://www.tarotnow.xyz/vi/reading |

## Duplicate Request Candidates (Non-telemetry)
| Scenario | Viewport | Feature | Route | Count | Request Key |
| --- | --- | --- | --- | ---: | --- |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/chunks/0kjazl8b8k9ly.css |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/chunks/0dg8~d_lbp93p.js |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/chunks/turbopack-164lj7fr~s_z4.js |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/chunks/07or0in3dm_w_.js |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/chunks/0a2n5k_101fwj.js |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/chunks/0kj4sun3~h8xd.js |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/chunks/0.ekkbyazw_22.js |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/chunks/0o3-.av4h_47m.js |

## Collection Image Metrics
| Scenario | Viewport | Image Requests | Image 400-800ms | Image >800ms | First-load Img Requests | Reopen Img Requests | 304 Cache Hits |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | 0 | 0 | 0 | 0 | 0 | 0 |
| logged-in-reader | desktop | 0 | 0 | 0 | 0 | 0 | 0 |
| logged-in-admin | mobile | 0 | 0 | 0 | 0 | 0 | 0 |
| logged-in-reader | mobile | 0 | 0 | 0 | 0 | 0 | 0 |

## Notes
- Duplicate `/cdn-cgi/rum` được xem là telemetry của Cloudflare, không coi là business over-fetch.
- Coverage dynamic route có thể bị `coverage-blocked` nếu environment không có dữ liệu phù hợp tại thời điểm chạy.
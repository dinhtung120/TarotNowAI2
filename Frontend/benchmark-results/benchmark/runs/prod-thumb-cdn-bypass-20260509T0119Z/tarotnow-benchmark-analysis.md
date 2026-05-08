# TarotNow Benchmark Analysis

- Run time (UTC): 2026-05-08T18:30:44.619Z
- Base: https://www.tarotnow.xyz/vi
- Benchmark mode: full-matrix
- Matrix: Chromium desktop + mobile, logged-out + logged-in-admin + logged-in-reader

## Scenario Summary
| Scenario | Viewport | Pages | Avg requests/page | Avg nav (ms) | Pending | Doc Reloads | Handshake Redirects | Session API Calls | Failed Requests | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 25.0 | 2927 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | desktop | 43 | 29.7 | 3023 | 0 | 0 | 1 | 12 | 1 | yes |
| logged-in-reader | desktop | 33 | 30.2 | 2991 | 0 | 0 | 1 | 21 | 0 | yes |
| logged-out | mobile | 9 | 25.0 | 2786 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | mobile | 43 | 29.7 | 2910 | 0 | 0 | 1 | 16 | 0 | yes |
| logged-in-reader | mobile | 33 | 29.3 | 2925 | 0 | 0 | 0 | 16 | 3 | yes |

## Route-Family Summary
| Scenario | Viewport | Route Family | Pages | Avg Requests/Page | Avg Navigation (ms) | Pending |
| --- | --- | --- | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | 10 | 29.4 | 2929 | 0 |
| logged-in-admin | desktop | chat | 1 | 28.0 | 2786 | 0 |
| logged-in-admin | desktop | collection | 1 | 30.0 | 6586 | 0 |
| logged-in-admin | desktop | community | 1 | 35.0 | 4058 | 0 |
| logged-in-admin | desktop | gacha | 2 | 30.0 | 2850 | 0 |
| logged-in-admin | desktop | gamification | 1 | 31.0 | 3261 | 0 |
| logged-in-admin | desktop | home | 1 | 26.0 | 2745 | 0 |
| logged-in-admin | desktop | inventory | 1 | 30.0 | 2843 | 0 |
| logged-in-admin | desktop | leaderboard | 1 | 32.0 | 3189 | 0 |
| logged-in-admin | desktop | legal | 3 | 25.0 | 2688 | 0 |
| logged-in-admin | desktop | notifications | 1 | 30.0 | 2882 | 0 |
| logged-in-admin | desktop | profile | 3 | 28.7 | 3039 | 0 |
| logged-in-admin | desktop | reader | 1 | 28.0 | 2770 | 0 |
| logged-in-admin | desktop | readers | 7 | 28.9 | 2837 | 0 |
| logged-in-admin | desktop | reading | 5 | 29.6 | 2860 | 0 |
| logged-in-admin | desktop | wallet | 4 | 36.0 | 3138 | 0 |
| logged-in-admin | mobile | admin | 10 | 29.2 | 2754 | 0 |
| logged-in-admin | mobile | chat | 1 | 28.0 | 2778 | 0 |
| logged-in-admin | mobile | collection | 1 | 31.0 | 5568 | 0 |
| logged-in-admin | mobile | community | 1 | 30.0 | 3546 | 0 |
| logged-in-admin | mobile | gacha | 2 | 31.5 | 2893 | 0 |
| logged-in-admin | mobile | gamification | 1 | 33.0 | 3407 | 0 |
| logged-in-admin | mobile | home | 1 | 26.0 | 2704 | 0 |
| logged-in-admin | mobile | inventory | 1 | 32.0 | 2832 | 0 |
| logged-in-admin | mobile | leaderboard | 1 | 30.0 | 2818 | 0 |
| logged-in-admin | mobile | legal | 3 | 25.0 | 2715 | 0 |
| logged-in-admin | mobile | notifications | 1 | 30.0 | 2854 | 0 |
| logged-in-admin | mobile | profile | 3 | 29.7 | 2907 | 0 |
| logged-in-admin | mobile | reader | 1 | 28.0 | 2776 | 0 |
| logged-in-admin | mobile | readers | 7 | 28.4 | 2827 | 0 |
| logged-in-admin | mobile | reading | 5 | 29.8 | 2793 | 0 |
| logged-in-admin | mobile | wallet | 4 | 35.5 | 2976 | 0 |
| logged-in-reader | desktop | chat | 1 | 28.0 | 2761 | 0 |
| logged-in-reader | desktop | collection | 1 | 29.0 | 6277 | 0 |
| logged-in-reader | desktop | community | 1 | 36.0 | 3607 | 0 |
| logged-in-reader | desktop | gacha | 2 | 33.0 | 2855 | 0 |
| logged-in-reader | desktop | gamification | 1 | 30.0 | 2863 | 0 |
| logged-in-reader | desktop | home | 1 | 26.0 | 2745 | 0 |
| logged-in-reader | desktop | inventory | 1 | 33.0 | 2856 | 0 |
| logged-in-reader | desktop | leaderboard | 1 | 30.0 | 2817 | 0 |
| logged-in-reader | desktop | legal | 3 | 25.0 | 2754 | 0 |
| logged-in-reader | desktop | notifications | 1 | 30.0 | 2894 | 0 |
| logged-in-reader | desktop | profile | 3 | 29.7 | 2978 | 0 |
| logged-in-reader | desktop | reader | 1 | 30.0 | 2977 | 0 |
| logged-in-reader | desktop | readers | 7 | 29.1 | 2816 | 0 |
| logged-in-reader | desktop | reading | 5 | 29.6 | 2820 | 0 |
| logged-in-reader | desktop | wallet | 4 | 35.5 | 3049 | 0 |
| logged-in-reader | mobile | chat | 1 | 28.0 | 2732 | 0 |
| logged-in-reader | mobile | collection | 1 | 29.0 | 5665 | 0 |
| logged-in-reader | mobile | community | 1 | 30.0 | 3608 | 0 |
| logged-in-reader | mobile | gacha | 2 | 31.0 | 2868 | 0 |
| logged-in-reader | mobile | gamification | 1 | 30.0 | 2835 | 0 |
| logged-in-reader | mobile | home | 1 | 26.0 | 2719 | 0 |
| logged-in-reader | mobile | inventory | 1 | 32.0 | 2875 | 0 |
| logged-in-reader | mobile | leaderboard | 1 | 30.0 | 2755 | 0 |
| logged-in-reader | mobile | legal | 3 | 25.0 | 2750 | 0 |
| logged-in-reader | mobile | notifications | 1 | 30.0 | 2900 | 0 |
| logged-in-reader | mobile | profile | 3 | 29.3 | 2848 | 0 |
| logged-in-reader | mobile | reader | 1 | 28.0 | 2757 | 0 |
| logged-in-reader | mobile | readers | 7 | 28.3 | 2745 | 0 |
| logged-in-reader | mobile | reading | 5 | 31.4 | 2842 | 0 |
| logged-in-reader | mobile | wallet | 4 | 31.0 | 2930 | 0 |
| logged-out | desktop | auth | 5 | 24.2 | 2872 | 0 |
| logged-out | desktop | home | 1 | 29.0 | 3740 | 0 |
| logged-out | desktop | legal | 3 | 25.0 | 2746 | 0 |
| logged-out | mobile | auth | 5 | 24.0 | 2743 | 0 |
| logged-out | mobile | home | 1 | 30.0 | 3151 | 0 |
| logged-out | mobile | legal | 3 | 25.0 | 2735 | 0 |

## Key Findings
1. Critical: 4 page(s) vượt 35 requests.
2. High: 142 page(s) vượt 25 requests.
3. High: 23 request vượt 800ms.
4. Medium: 289 request trong dải 400-800ms.
5. High: phát hiện 3 handshake redirect(s), cần kiểm tra vòng lặp auth/session.
6. Collection-focus: 36 image request(s) >800ms trên 62 image request(s).

## Top Slow Pages
| Scenario | Viewport | Feature | Route | Navigate (ms) | Request count | LCP (ms) | TBT (ms) | CLS |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | 6586 | 30 | 1048 | 0.0 | 0.0042 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 6277 | 29 | 496 | 0.0 | 0.0040 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 5665 | 29 | 752 | 0.0 | 0.0000 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 5568 | 31 | 780 | 0.0 | 0.0000 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 4172 | 60 | 1460 | 32.0 | 0.0000 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | 4058 | 35 | 1872 | 0.0 | 0.0041 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 3771 | 56 | 1332 | 34.0 | 0.0000 |
| logged-out | desktop | auth-public | /vi | 3740 | 29 | 1224 | 286.0 | 0.0000 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 3676 | 56 | 1096 | 0.0 | 0.0000 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | 3608 | 30 | 1660 | 0.0 | 0.0051 |

## High Slow Requests (> 800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | GET | 200 | 1112 | 409 | https://www.tarotnow.xyz/vi/profile |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile | GET | 200 | 1070 | 315 | https://www.tarotnow.xyz/vi/profile |
| logged-out | desktop | auth-public | /vi | GET | 200 | 1053 | 371 | https://www.tarotnow.xyz/vi |
| logged-out | desktop | auth-public | /vi | GET | 200 | 1028 | 547 | https://www.tarotnow.xyz/_next/static/chunks/0kxzca98t52gs.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 1018 | 548 | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-out | mobile | auth-public | /vi | GET | 200 | 1001 | 583 | https://www.tarotnow.xyz/vi |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | GET | 200 | 996 | 316 | https://www.tarotnow.xyz/vi/gamification |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 968 | 506 | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1920&q=75 |
| logged-in-reader | desktop | reader-chat | /vi/reader/apply | GET | 200 | 872 | 335 | https://www.tarotnow.xyz/vi/reader/apply |
| logged-in-admin | desktop | reading | /vi/reading | GET | 200 | 864 | 379 | https://www.tarotnow.xyz/vi/reading |
| logged-in-reader | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | GET | 200 | 861 | 331 | https://www.tarotnow.xyz/vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | GET | 200 | 845 | 324 | https://www.tarotnow.xyz/vi/wallet/deposit |
| logged-out | desktop | auth-public | /vi/login | GET | 200 | 826 | 342 | https://www.tarotnow.xyz/_next/static/chunks/0kxzca98t52gs.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 817 | 315 | https://www.tarotnow.xyz/vi/notifications |
| logged-in-admin | desktop | admin | /vi/admin/deposits | GET | 200 | 816 | 345 | https://www.tarotnow.xyz/vi/admin/deposits |

## Medium Slow Requests (400-800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- |
| logged-in-reader | mobile | reading | /vi/reading | GET | 200 | 800 | 317 | https://www.tarotnow.xyz/vi/reading |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | GET | 200 | 799 | 317 | https://www.tarotnow.xyz/vi/notifications |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | GET | 200 | 799 | 324 | https://www.tarotnow.xyz/vi/wallet |
| logged-out | desktop | auth-public | /vi | GET | 200 | 798 | 553 | https://www.tarotnow.xyz/_next/static/chunks/0u95sv2h1cu3o.js |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | GET | 200 | 798 | 311 | https://www.tarotnow.xyz/vi/gacha |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 798 | 298 | https://www.tarotnow.xyz/vi/readers/97410d49-ab32-469d-b421-f57321b262a2 |
| logged-in-admin | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 797 | 325 | https://www.tarotnow.xyz/vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | GET | 200 | 797 | 319 | https://www.tarotnow.xyz/vi/gamification |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | GET | 200 | 796 | 310 | https://www.tarotnow.xyz/vi/gacha |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 795 | 320 | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-admin | desktop | admin | /vi/admin | GET | 200 | 795 | 550 | https://www.tarotnow.xyz/_next/static/chunks/16o0.rxdplbck.js |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | GET | 200 | 795 | 310 | https://www.tarotnow.xyz/vi/inventory |
| logged-in-admin | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 795 | 335 | https://www.tarotnow.xyz/vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 |
| logged-out | desktop | auth-public | /vi/verify-email | GET | 200 | 792 | 392 | https://www.tarotnow.xyz/vi/verify-email |
| logged-in-reader | mobile | reader-chat | /vi/readers | GET | 200 | 789 | 310 | https://www.tarotnow.xyz/vi/readers |

## Duplicate Request Candidates (Non-telemetry)
| Scenario | Viewport | Feature | Route | Count | Request Key |
| --- | --- | --- | --- | ---: | --- |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0kjazl8b8k9ly.css |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0dg8~d_lbp93p.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/turbopack-164lj7fr~s_z4.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/07or0in3dm_w_.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0a2n5k_101fwj.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0kj4sun3~h8xd.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0kxzca98t52gs.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0u95sv2h1cu3o.js |

## Collection Image Metrics
| Scenario | Viewport | Image Requests | Image 400-800ms | Image >800ms | First-load Img Requests | Reopen Img Requests | 304 Cache Hits |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | 21 | 3 | 13 | 20 | 1 | 0 |
| logged-in-reader | desktop | 11 | 6 | 3 | 11 | 0 | 0 |
| logged-in-admin | mobile | 20 | 5 | 13 | 19 | 1 | 0 |
| logged-in-reader | mobile | 10 | 2 | 7 | 10 | 0 | 0 |

## Notes
- Duplicate `/cdn-cgi/rum` được xem là telemetry của Cloudflare, không coi là business over-fetch.
- Coverage dynamic route có thể bị `coverage-blocked` nếu environment không có dữ liệu phù hợp tại thời điểm chạy.
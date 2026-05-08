# TarotNow Navigation Benchmark Report

- Generated at (UTC): 2026-05-08T18:30:44.619Z
- Base URL: https://www.tarotnow.xyz/vi
- Benchmark mode: full-matrix
- Thresholds: >35 requests = Critical, >25 = High, request >800ms = High, >400ms = Medium
- Critical pages (request count): 4
- High pages (request count): 142
- High slow requests: 23
- Medium slow requests: 289

## Scenario Summary
| Scenario | Viewport | Pages Benchmarked | Avg Navigation (ms) | Total Requests | Pending Requests | Document Reloads | Session API Calls | Failed Requests | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 2927 | 225 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | desktop | 43 | 3023 | 1279 | 0 | 0 | 12 | 1 | yes |
| logged-in-reader | desktop | 33 | 2991 | 996 | 0 | 0 | 21 | 0 | yes |
| logged-out | mobile | 9 | 2786 | 225 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | mobile | 43 | 2910 | 1277 | 0 | 0 | 16 | 0 | yes |
| logged-in-reader | mobile | 33 | 2925 | 967 | 0 | 0 | 16 | 3 | yes |

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

## Per-Page Metrics
| Scenario | Viewport | Feature | Route | Requests | Interaction Requests | Severity | Doc Reloads | Handshake Redirects | Session API Calls | Failed Requests | Collection Img Requests | Collection Img 400-800ms | Collection Img >800ms | Collection Img First Load | Collection Img Reopen | Collection Img Cache Hits | Navigate (ms) | DOMContentLoaded (ms) | Load (ms) | FCP (ms) | LCP (ms) | CLS | TBT (ms) | Transfer Bytes |
| --- | --- | --- | --- | ---: | ---: | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-out | desktop | auth-public | /vi | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3740 | 1405 | 1727 | 1224 | 1224 | 0.0000 | 286.0 | 601595 |
| logged-out | desktop | auth-public | /vi/login | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3270 | 1086 | 1265 | 1108 | 1108 | 0.0000 | 0.0 | 512407 |
| logged-out | desktop | auth-public | /vi/register | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2688 | 679 | 679 | 552 | 552 | 0.0000 | 0.0 | 513594 |
| logged-out | desktop | auth-public | /vi/forgot-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2720 | 715 | 715 | 536 | 536 | 0.0000 | 0.0 | 512004 |
| logged-out | desktop | auth-public | /vi/reset-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2823 | 818 | 818 | 580 | 580 | 0.0000 | 0.0 | 512158 |
| logged-out | desktop | auth-public | /vi/verify-email | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2861 | 854 | 854 | 752 | 752 | 0.0000 | 0.0 | 512195 |
| logged-out | desktop | auth-public | /vi/legal/tos | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2718 | 710 | 710 | 592 | 592 | 0.0000 | 0.0 | 525850 |
| logged-out | desktop | auth-public | /vi/legal/privacy | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2747 | 741 | 741 | 468 | 468 | 0.0000 | 0.0 | 525980 |
| logged-out | desktop | auth-public | /vi/legal/ai-disclaimer | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2773 | 763 | 763 | 532 | 860 | 0.0000 | 0.0 | 526227 |
| logged-in-admin | desktop | auth-public | /vi | 26 | 13 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2745 | 733 | 733 | 596 | 1108 | 0.0035 | 186.0 | 537626 |
| logged-in-admin | desktop | reading | /vi/reading | 29 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2968 | 942 | 949 | 596 | 1028 | 0.0041 | 0.0 | 642942 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | 30 | 10 | high | 0 | 0 | 0 | 0 | 5 | 4 | 1 | 5 | 0 | 0 | 2843 | 831 | 831 | 532 | 1208 | 0.0041 | 0.0 | 644926 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | 30 | 12 | high | 0 | 0 | 0 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2857 | 851 | 851 | 660 | 1084 | 0.0041 | 0.0 | 725095 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | 30 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2843 | 836 | 837 | 616 | 1196 | 0.0041 | 0.0 | 726343 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | 30 | 35 | high | 0 | 0 | 1 | 0 | 21 | 3 | 13 | 20 | 1 | 0 | 6586 | 741 | 1209 | 708 | 1048 | 0.0042 | 0.0 | 644424 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | 30 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3474 | 1166 | 1466 | 1160 | 1584 | 0.0489 | 0.0 | 635480 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2864 | 851 | 854 | 628 | 996 | 0.0041 | 0.0 | 631674 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | 41 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2779 | 836 | 976 | 584 | 956 | 0.0489 | 0.0 | 630901 |
| logged-in-admin | desktop | reader-chat | /vi/readers | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2866 | 856 | 856 | 504 | 1164 | 0.0041 | 0.0 | 636248 |
| logged-in-admin | desktop | reader-chat | /vi/chat | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2786 | 761 | 777 | 532 | 960 | 0.0041 | 0.0 | 631713 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/leaderboard | 32 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3189 | 709 | 1184 | 624 | 1012 | 0.0179 | 0.0 | 652409 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | 35 | 5 | high | 0 | 0 | 1 | 0 | 1 | 0 | 1 | 1 | 0 | 0 | 4058 | 872 | 1282 | 544 | 1872 | 0.0041 | 0.0 | 777860 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/gamification | 31 | 9 | high | 0 | 0 | 1 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 3261 | 868 | 1255 | 528 | 912 | 0.0176 | 0.0 | 643999 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 60 | 1 | critical | 0 | 1 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4172 | 1130 | 1368 | 864 | 1460 | 0.0000 | 32.0 | 1207221 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2752 | 726 | 743 | 572 | 984 | 0.0041 | 0.0 | 631681 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2774 | 740 | 765 | 572 | 1156 | 0.0041 | 0.0 | 632333 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 28 | 39 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2854 | 850 | 850 | 552 | 1264 | 0.0041 | 0.0 | 631143 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/notifications | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2882 | 826 | 872 | 524 | 904 | 0.0046 | 0.0 | 634350 |
| logged-in-admin | desktop | reader-chat | /vi/reader/apply | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2770 | 742 | 762 | 524 | 876 | 0.0041 | 0.0 | 632563 |
| logged-in-admin | desktop | reading | /vi/reading/history | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2743 | 711 | 733 | 512 | 1116 | 0.0041 | 0.0 | 633104 |
| logged-in-admin | desktop | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2738 | 731 | 731 | 620 | 940 | 0.0020 | 0.0 | 526323 |
| logged-in-admin | desktop | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2657 | 648 | 648 | 512 | 804 | 0.0020 | 0.0 | 526297 |
| logged-in-admin | desktop | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2669 | 663 | 663 | 616 | 952 | 0.0020 | 0.0 | 526416 |
| logged-in-admin | desktop | admin | /vi/admin | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3179 | 786 | 1169 | 552 | 900 | 0.0000 | 0.0 | 647625 |
| logged-in-admin | desktop | admin | /vi/admin/deposits | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2892 | 862 | 885 | 516 | 1068 | 0.0000 | 0.0 | 647860 |
| logged-in-admin | desktop | admin | /vi/admin/disputes | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2844 | 816 | 829 | 520 | 920 | 0.0000 | 0.0 | 646013 |
| logged-in-admin | desktop | admin | /vi/admin/gamification | 32 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3242 | 743 | 1234 | 520 | 808 | 0.0022 | 0.0 | 698654 |
| logged-in-admin | desktop | admin | /vi/admin/promotions | 28 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2868 | 848 | 860 | 540 | 904 | 0.0000 | 0.0 | 644754 |
| logged-in-admin | desktop | admin | /vi/admin/reader-requests | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2806 | 798 | 799 | 672 | 984 | 0.0000 | 0.0 | 646596 |
| logged-in-admin | desktop | admin | /vi/admin/readings | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2845 | 824 | 839 | 572 | 1076 | 0.0000 | 0.0 | 648820 |
| logged-in-admin | desktop | admin | /vi/admin/system-configs | 30 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2937 | 841 | 929 | 548 | 1004 | 0.0000 | 0.0 | 689565 |
| logged-in-admin | desktop | admin | /vi/admin/users | 30 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2857 | 848 | 850 | 536 | 1220 | 0.0000 | 0.0 | 650142 |
| logged-in-admin | desktop | admin | /vi/admin/withdrawals | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2815 | 790 | 807 | 588 | 880 | 0.0000 | 0.0 | 646213 |
| logged-in-admin | desktop | reading | /vi/reading/session/60daf472-49db-4823-9fd9-2bdc883e1923 | 28 | 10 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2867 | 839 | 861 | 588 | 1008 | 0.0041 | 0.0 | 632258 |
| logged-in-admin | desktop | reading | /vi/reading/session/9d89d38d-8ea2-4420-b477-d1cd83d81a7b | 28 | 11 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2780 | 757 | 772 | 656 | 1048 | 0.0041 | 0.0 | 632170 |
| logged-in-admin | desktop | reading | /vi/reading/session/f335007c-d308-482f-b651-b34e2b32a50b | 35 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2942 | 764 | 934 | 508 | 912 | 0.0041 | 0.0 | 727021 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2698 | 672 | 691 | 524 | 900 | 0.0041 | 0.0 | 631293 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2823 | 759 | 813 | 528 | 880 | 0.0041 | 0.0 | 631441 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2861 | 830 | 847 | 732 | 1160 | 0.0041 | 0.0 | 631145 |
| logged-in-admin | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2903 | 880 | 886 | 528 | 1116 | 0.0041 | 0.0 | 635252 |
| logged-in-admin | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2828 | 753 | 809 | 508 | 1036 | 0.0041 | 0.0 | 632878 |
| logged-in-admin | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2877 | 839 | 854 | 544 | 1036 | 0.0000 | 0.0 | 635393 |
| logged-in-reader | desktop | auth-public | /vi | 26 | 13 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2745 | 735 | 735 | 528 | 1084 | 0.0033 | 179.0 | 537582 |
| logged-in-reader | desktop | reading | /vi/reading | 29 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2863 | 842 | 850 | 484 | 940 | 0.0039 | 0.0 | 643186 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | 33 | 8 | high | 0 | 0 | 2 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2856 | 846 | 846 | 492 | 1148 | 0.0039 | 0.0 | 647947 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | 33 | 9 | high | 0 | 0 | 2 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2862 | 853 | 853 | 488 | 1156 | 0.0039 | 0.0 | 728521 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | 33 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2848 | 838 | 838 | 492 | 1240 | 0.0039 | 0.0 | 727487 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 29 | 31 | high | 0 | 0 | 0 | 0 | 11 | 6 | 3 | 11 | 0 | 0 | 6277 | 709 | 710 | 496 | 496 | 0.0040 | 0.0 | 642303 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile | 33 | 4 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3221 | 1135 | 1211 | 480 | 1272 | 0.0726 | 0.0 | 639249 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2811 | 749 | 804 | 472 | 900 | 0.0039 | 0.0 | 631765 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2902 | 828 | 890 | 568 | 1316 | 0.0039 | 0.0 | 632749 |
| logged-in-reader | desktop | reader-chat | /vi/readers | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2867 | 860 | 860 | 520 | 1180 | 0.0039 | 0.0 | 636303 |
| logged-in-reader | desktop | reader-chat | /vi/chat | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2761 | 739 | 751 | 492 | 920 | 0.0039 | 0.0 | 631639 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | 30 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2817 | 765 | 808 | 512 | 868 | 0.0177 | 0.0 | 650051 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | 36 | 3 | critical | 0 | 0 | 1 | 0 | 1 | 1 | 0 | 1 | 0 | 0 | 3607 | 756 | 866 | 516 | 1656 | 0.0039 | 0.0 | 778946 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | 30 | 7 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2863 | 846 | 846 | 508 | 940 | 0.1238 | 0.0 | 643317 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet | 28 | 13 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2787 | 765 | 778 | 532 | 896 | 0.0094 | 0.0 | 632099 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 56 | 5 | critical | 0 | 1 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3771 | 1009 | 1010 | 792 | 1332 | 0.0000 | 34.0 | 1135420 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2787 | 763 | 779 | 524 | 908 | 0.0039 | 0.0 | 631838 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 30 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2852 | 841 | 842 | 488 | 1152 | 0.0095 | 0.0 | 635498 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2894 | 850 | 874 | 528 | 972 | 0.0040 | 0.0 | 634521 |
| logged-in-reader | desktop | reader-chat | /vi/reader/apply | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2977 | 920 | 969 | 520 | 1024 | 0.0039 | 0.0 | 634847 |
| logged-in-reader | desktop | reading | /vi/reading/history | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2848 | 800 | 826 | 488 | 1004 | 0.0039 | 0.0 | 635489 |
| logged-in-reader | desktop | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2786 | 775 | 775 | 528 | 840 | 0.0019 | 0.0 | 526343 |
| logged-in-reader | desktop | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2701 | 692 | 692 | 508 | 816 | 0.0019 | 0.0 | 526410 |
| logged-in-reader | desktop | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2775 | 766 | 766 | 552 | 856 | 0.0019 | 0.0 | 526454 |
| logged-in-reader | desktop | reading | /vi/reading/session/efd7e84f-9903-48ac-804d-6b36bb38b044 | 28 | 10 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2773 | 751 | 761 | 512 | 920 | 0.0039 | 0.0 | 632276 |
| logged-in-reader | desktop | reading | /vi/reading/session/e9280543-e216-42d0-bcb7-cc10b2e8fad7 | 33 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2867 | 749 | 857 | 492 | 908 | 0.0049 | 0.0 | 715402 |
| logged-in-reader | desktop | reading | /vi/reading/session/ac1b0435-eb90-4f53-a70f-15c12a1bb5ba | 28 | 11 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2748 | 721 | 736 | 532 | 916 | 0.0039 | 0.0 | 632298 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2708 | 690 | 698 | 496 | 888 | 0.0039 | 0.0 | 631407 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2733 | 707 | 721 | 496 | 832 | 0.0039 | 0.0 | 631420 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2894 | 838 | 878 | 560 | 904 | 0.0039 | 0.0 | 633465 |
| logged-in-reader | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2701 | 676 | 691 | 472 | 1068 | 0.0039 | 0.0 | 632807 |
| logged-in-reader | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2955 | 925 | 927 | 524 | 1160 | 0.0000 | 0.0 | 635173 |
| logged-in-reader | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2851 | 806 | 840 | 500 | 1096 | 0.0039 | 0.0 | 635559 |
| logged-out | mobile | auth-public | /vi | 30 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3151 | 1140 | 1140 | 972 | 972 | 0.0000 | 0.0 | 602797 |
| logged-out | mobile | auth-public | /vi/login | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2756 | 746 | 746 | 620 | 620 | 0.0000 | 0.0 | 512485 |
| logged-out | mobile | auth-public | /vi/register | 24 | 2 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2805 | 794 | 794 | 584 | 584 | 0.0000 | 0.0 | 512908 |
| logged-out | mobile | auth-public | /vi/forgot-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2781 | 769 | 769 | 548 | 548 | 0.0000 | 0.0 | 512074 |
| logged-out | mobile | auth-public | /vi/reset-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2702 | 694 | 694 | 476 | 476 | 0.0000 | 0.0 | 512102 |
| logged-out | mobile | auth-public | /vi/verify-email | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2669 | 660 | 661 | 480 | 480 | 0.0000 | 0.0 | 512226 |
| logged-out | mobile | auth-public | /vi/legal/tos | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2704 | 690 | 691 | 448 | 756 | 0.0000 | 0.0 | 525917 |
| logged-out | mobile | auth-public | /vi/legal/privacy | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2708 | 694 | 694 | 500 | 500 | 0.0000 | 0.0 | 526101 |
| logged-out | mobile | auth-public | /vi/legal/ai-disclaimer | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2794 | 780 | 780 | 648 | 648 | 0.0000 | 0.0 | 526154 |
| logged-in-admin | mobile | auth-public | /vi | 26 | 13 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2704 | 694 | 694 | 548 | 896 | 0.0032 | 0.0 | 537739 |
| logged-in-admin | mobile | reading | /vi/reading | 31 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2853 | 838 | 839 | 444 | 808 | 0.0000 | 0.0 | 645377 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | 32 | 8 | high | 0 | 0 | 1 | 0 | 5 | 5 | 0 | 5 | 0 | 0 | 2832 | 821 | 821 | 424 | 1060 | 0.0000 | 0.0 | 646918 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | 30 | 11 | high | 0 | 0 | 0 | 0 | 5 | 4 | 1 | 5 | 0 | 0 | 2922 | 910 | 911 | 432 | 1136 | 0.0000 | 0.0 | 725169 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha/history | 33 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2864 | 847 | 848 | 444 | 904 | 0.0000 | 0.0 | 729626 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 31 | 32 | high | 0 | 0 | 1 | 0 | 20 | 5 | 13 | 19 | 1 | 0 | 5568 | 807 | 829 | 444 | 780 | 0.0000 | 0.0 | 645700 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | 31 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3115 | 846 | 1105 | 440 | 1116 | 0.0689 | 0.0 | 636788 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2743 | 732 | 734 | 448 | 768 | 0.0000 | 0.0 | 632083 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | 30 | 38 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2864 | 866 | 920 | 468 | 1120 | 0.0760 | 0.0 | 633413 |
| logged-in-admin | mobile | reader-chat | /vi/readers | 29 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2858 | 823 | 824 | 580 | 948 | 0.0000 | 0.0 | 635261 |
| logged-in-admin | mobile | reader-chat | /vi/chat | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2778 | 754 | 762 | 448 | 756 | 0.0000 | 0.0 | 631695 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/leaderboard | 30 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2818 | 767 | 804 | 444 | 772 | 0.0196 | 0.0 | 649909 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | 30 | 10 | high | 0 | 0 | 0 | 0 | 1 | 0 | 1 | 1 | 0 | 0 | 3546 | 801 | 807 | 688 | 2040 | 0.0051 | 0.0 | 643325 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 33 | 4 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3407 | 1394 | 1394 | 984 | 1328 | 0.0000 | 0.0 | 646599 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | 28 | 14 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2687 | 672 | 674 | 432 | 764 | 0.0000 | 0.0 | 631875 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 56 | 5 | critical | 0 | 1 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3676 | 966 | 966 | 720 | 1096 | 0.0000 | 0.0 | 1135614 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2716 | 700 | 700 | 420 | 772 | 0.0000 | 0.0 | 632410 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 30 | 37 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2826 | 818 | 818 | 612 | 960 | 0.0071 | 0.0 | 633416 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2854 | 810 | 831 | 456 | 812 | 0.0000 | 0.0 | 634475 |
| logged-in-admin | mobile | reader-chat | /vi/reader/apply | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2776 | 758 | 763 | 484 | 836 | 0.0000 | 0.0 | 632793 |
| logged-in-admin | mobile | reading | /vi/reading/history | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2860 | 842 | 850 | 448 | 932 | 0.0000 | 0.0 | 633084 |
| logged-in-admin | mobile | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2736 | 726 | 726 | 456 | 784 | 0.0032 | 0.0 | 526308 |
| logged-in-admin | mobile | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2735 | 726 | 726 | 480 | 796 | 0.0032 | 0.0 | 526274 |
| logged-in-admin | mobile | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2674 | 665 | 665 | 472 | 780 | 0.0032 | 0.0 | 526545 |
| logged-in-admin | mobile | admin | /vi/admin | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2773 | 751 | 756 | 432 | 760 | 0.0000 | 0.0 | 647826 |
| logged-in-admin | mobile | admin | /vi/admin/deposits | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2859 | 831 | 836 | 508 | 876 | 0.0000 | 0.0 | 647630 |
| logged-in-admin | mobile | admin | /vi/admin/disputes | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2679 | 668 | 668 | 424 | 736 | 0.0000 | 0.0 | 646097 |
| logged-in-admin | mobile | admin | /vi/admin/gamification | 30 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2818 | 803 | 803 | 572 | 884 | 0.0000 | 0.0 | 664736 |
| logged-in-admin | mobile | admin | /vi/admin/promotions | 28 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2735 | 725 | 725 | 484 | 792 | 0.0000 | 0.0 | 644892 |
| logged-in-admin | mobile | admin | /vi/admin/reader-requests | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2693 | 680 | 681 | 448 | 776 | 0.0000 | 0.0 | 646543 |
| logged-in-admin | mobile | admin | /vi/admin/readings | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2731 | 719 | 720 | 548 | 868 | 0.0000 | 0.0 | 648921 |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | 30 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2848 | 765 | 836 | 444 | 776 | 0.0000 | 0.0 | 689590 |
| logged-in-admin | mobile | admin | /vi/admin/users | 30 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2704 | 687 | 687 | 432 | 752 | 0.0000 | 0.0 | 650148 |
| logged-in-admin | mobile | admin | /vi/admin/withdrawals | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2699 | 684 | 684 | 432 | 740 | 0.0000 | 0.0 | 646224 |
| logged-in-admin | mobile | reading | /vi/reading/session/491fbd8e-0ff1-4d60-9edc-786f508c66f5 | 28 | 10 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2692 | 679 | 680 | 524 | 852 | 0.0000 | 0.0 | 632397 |
| logged-in-admin | mobile | reading | /vi/reading/session/6134d2b7-29af-4706-bdca-3f807b84a4a9 | 34 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2880 | 726 | 869 | 428 | 760 | 0.0000 | 0.0 | 694930 |
| logged-in-admin | mobile | reading | /vi/reading/session/9bbdb4cc-e46e-4164-9a06-a8fdd18887cd | 28 | 10 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2678 | 666 | 667 | 432 | 756 | 0.0000 | 0.0 | 632550 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2783 | 765 | 773 | 440 | 760 | 0.0000 | 0.0 | 631386 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2828 | 807 | 809 | 448 | 776 | 0.0000 | 0.0 | 633482 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2787 | 767 | 774 | 448 | 772 | 0.0000 | 0.0 | 631377 |
| logged-in-admin | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2875 | 857 | 857 | 584 | 924 | 0.0000 | 0.0 | 632983 |
| logged-in-admin | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2769 | 750 | 758 | 424 | 828 | 0.0000 | 0.0 | 632950 |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2890 | 868 | 876 | 476 | 856 | 0.0000 | 0.0 | 633165 |
| logged-in-reader | mobile | auth-public | /vi | 26 | 13 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2719 | 708 | 708 | 452 | 816 | 0.0032 | 0.0 | 537853 |
| logged-in-reader | mobile | reading | /vi/reading | 31 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2886 | 858 | 863 | 452 | 840 | 0.0000 | 0.0 | 645005 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/inventory | 32 | 8 | high | 0 | 0 | 1 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2875 | 860 | 860 | 464 | 1084 | 0.0000 | 0.0 | 646874 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | 30 | 12 | high | 0 | 0 | 0 | 0 | 5 | 2 | 0 | 5 | 0 | 0 | 2864 | 850 | 850 | 444 | 1084 | 0.0000 | 0.0 | 725157 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | 32 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2872 | 857 | 857 | 456 | 1116 | 0.0000 | 0.0 | 726620 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 29 | 29 | high | 0 | 0 | 0 | 0 | 10 | 2 | 7 | 10 | 0 | 0 | 5665 | 761 | 791 | 444 | 752 | 0.0000 | 0.0 | 642262 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | 32 | 5 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2963 | 868 | 953 | 432 | 1048 | 0.0821 | 0.0 | 637765 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2774 | 751 | 762 | 476 | 800 | 0.0000 | 0.0 | 631738 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2806 | 784 | 793 | 448 | 788 | 0.0000 | 0.0 | 632495 |
| logged-in-reader | mobile | reader-chat | /vi/readers | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2858 | 843 | 843 | 460 | 1072 | 0.0000 | 0.0 | 636333 |
| logged-in-reader | mobile | reader-chat | /vi/chat | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2732 | 710 | 722 | 424 | 764 | 0.0000 | 0.0 | 632044 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | 30 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2755 | 714 | 731 | 448 | 760 | 0.0196 | 0.0 | 650221 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | 30 | 9 | high | 0 | 0 | 0 | 0 | 1 | 1 | 0 | 1 | 0 | 0 | 3608 | 774 | 788 | 516 | 1660 | 0.0051 | 0.0 | 643391 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/gamification | 30 | 7 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2835 | 821 | 821 | 440 | 816 | 0.0000 | 0.0 | 643372 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2857 | 841 | 841 | 444 | 1092 | 0.0000 | 0.0 | 636501 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | 34 | 7 | high | 0 | 0 | 1 | 3 | 0 | 0 | 0 | 0 | 0 | 0 | 2949 | 892 | 916 | 484 | 900 | 0.0032 | 0.0 | 635166 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2830 | 804 | 807 | 456 | 788 | 0.0000 | 0.0 | 634331 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 30 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3084 | 1068 | 1068 | 664 | 1292 | 0.0330 | 0.0 | 635380 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2900 | 880 | 881 | 468 | 836 | 0.0000 | 0.0 | 634533 |
| logged-in-reader | mobile | reader-chat | /vi/reader/apply | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2757 | 728 | 744 | 448 | 788 | 0.0000 | 0.0 | 632429 |
| logged-in-reader | mobile | reading | /vi/reading/history | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2798 | 758 | 780 | 432 | 760 | 0.0000 | 0.0 | 635212 |
| logged-in-reader | mobile | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2746 | 731 | 732 | 444 | 756 | 0.0032 | 0.0 | 526267 |
| logged-in-reader | mobile | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2749 | 736 | 736 | 488 | 796 | 0.0032 | 0.0 | 526257 |
| logged-in-reader | mobile | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2754 | 739 | 739 | 612 | 920 | 0.0032 | 0.0 | 526514 |
| logged-in-reader | mobile | reading | /vi/reading/session/b5b51196-e110-4046-9a8d-05e196b6aa1f | 34 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2904 | 779 | 890 | 448 | 796 | 0.0000 | 0.0 | 695202 |
| logged-in-reader | mobile | reading | /vi/reading/session/0cbbf7e2-7126-4cc2-ba5b-2e92e340041b | 34 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2920 | 775 | 906 | 448 | 780 | 0.0000 | 0.0 | 695191 |
| logged-in-reader | mobile | reading | /vi/reading/session/1796af3f-42f6-4466-9c2e-8f5e9dc72105 | 28 | 10 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2702 | 675 | 690 | 444 | 780 | 0.0000 | 0.0 | 632419 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2708 | 681 | 695 | 436 | 768 | 0.0000 | 0.0 | 631363 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2719 | 692 | 702 | 444 | 780 | 0.0000 | 0.0 | 631264 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2681 | 656 | 666 | 448 | 768 | 0.0000 | 0.0 | 631529 |
| logged-in-reader | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2725 | 692 | 711 | 436 | 852 | 0.0000 | 0.0 | 633159 |
| logged-in-reader | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2766 | 740 | 751 | 480 | 880 | 0.0000 | 0.0 | 633354 |
| logged-in-reader | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2757 | 725 | 744 | 452 | 848 | 0.0000 | 0.0 | 633430 |

## Collection Image Focus
| Scenario | Viewport | Route | Image Requests | Image 400-800ms | Image >800ms | First-load Img Requests | Reopen Img Requests | 304 Cache Hits |
| --- | --- | --- | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | /vi/collection | 21 | 3 | 13 | 20 | 1 | 0 |
| logged-in-reader | desktop | /vi/collection | 11 | 6 | 3 | 11 | 0 | 0 |
| logged-in-admin | mobile | /vi/collection | 20 | 5 | 13 | 19 | 1 | 0 |
| logged-in-reader | mobile | /vi/collection | 10 | 2 | 7 | 10 | 0 | 0 |

## Suspicious Pages (>25 requests)
| Scenario | Viewport | Feature | Route | Request Count | Severity | API | Static | Third-party |
| --- | --- | --- | --- | ---: | --- | ---: | ---: | ---: |
| logged-out | desktop | auth-public | /vi | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | auth-public | /vi | 26 | high | 0 | 24 | 0 |
| logged-in-admin | desktop | reading | /vi/reading | 29 | high | 1 | 26 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | 30 | high | 0 | 28 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | 30 | high | 0 | 28 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | 30 | high | 0 | 28 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | 30 | high | 1 | 27 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | 30 | high | 1 | 27 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers | 30 | high | 2 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/leaderboard | 32 | high | 2 | 28 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | 35 | high | 1 | 32 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/gamification | 31 | high | 2 | 27 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 60 | critical | 4 | 52 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/notifications | 30 | high | 2 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | admin | /vi/admin | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/deposits | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/disputes | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/gamification | 32 | high | 0 | 30 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/promotions | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/reader-requests | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/readings | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/system-configs | 30 | high | 0 | 28 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/users | 30 | high | 0 | 28 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/withdrawals | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/60daf472-49db-4823-9fd9-2bdc883e1923 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/9d89d38d-8ea2-4420-b477-d1cd83d81a7b | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/f335007c-d308-482f-b651-b34e2b32a50b | 35 | high | 3 | 30 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 30 | high | 2 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 30 | high | 2 | 26 | 0 |
| logged-in-reader | desktop | auth-public | /vi | 26 | high | 0 | 24 | 0 |
| logged-in-reader | desktop | reading | /vi/reading | 29 | high | 1 | 26 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | 33 | high | 3 | 28 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | 33 | high | 3 | 28 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | 33 | high | 3 | 28 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile | 33 | high | 3 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers | 30 | high | 2 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | 30 | high | 0 | 28 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | 36 | critical | 2 | 32 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | 30 | high | 1 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 56 | critical | 3 | 49 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 30 | high | 2 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | 30 | high | 2 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/reader/apply | 30 | high | 2 | 26 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/history | 30 | high | 2 | 26 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/efd7e84f-9903-48ac-804d-6b36bb38b044 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/e9280543-e216-42d0-bcb7-cc10b2e8fad7 | 33 | high | 2 | 29 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/ac1b0435-eb90-4f53-a70f-15c12a1bb5ba | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 30 | high | 2 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 30 | high | 2 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 30 | high | 2 | 26 | 0 |
| logged-out | mobile | auth-public | /vi | 30 | high | 1 | 27 | 0 |
| logged-in-admin | mobile | auth-public | /vi | 26 | high | 0 | 24 | 0 |
| logged-in-admin | mobile | reading | /vi/reading | 31 | high | 3 | 26 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | 32 | high | 2 | 28 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha/history | 33 | high | 3 | 28 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 31 | high | 2 | 27 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | 31 | high | 2 | 27 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | 30 | high | 2 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers | 29 | high | 1 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/leaderboard | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 33 | high | 3 | 27 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 56 | critical | 3 | 49 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 30 | high | 2 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | 30 | high | 2 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | admin | /vi/admin | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/deposits | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/disputes | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/gamification | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/promotions | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/reader-requests | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/readings | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/users | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/withdrawals | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/491fbd8e-0ff1-4d60-9edc-786f508c66f5 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/6134d2b7-29af-4706-bdca-3f807b84a4a9 | 34 | high | 3 | 29 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/9bbdb4cc-e46e-4164-9a06-a8fdd18887cd | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 30 | high | 2 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | auth-public | /vi | 26 | high | 0 | 24 | 0 |
| logged-in-reader | mobile | reading | /vi/reading | 31 | high | 3 | 26 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/inventory | 32 | high | 2 | 28 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | 30 | high | 0 | 28 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | 32 | high | 2 | 28 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | 32 | high | 3 | 27 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers | 30 | high | 2 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | 30 | high | 0 | 28 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | 30 | high | 0 | 28 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/gamification | 30 | high | 1 | 27 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 30 | high | 2 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | 34 | high | 5 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 30 | high | 2 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 30 | high | 2 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | 30 | high | 2 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/history | 30 | high | 2 | 26 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/b5b51196-e110-4046-9a8d-05e196b6aa1f | 34 | high | 3 | 29 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/0cbbf7e2-7126-4cc2-ba5b-2e92e340041b | 34 | high | 3 | 29 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/1796af3f-42f6-4466-9c2e-8f5e9dc72105 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |

## High Slow Requests (>800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | GET | 200 | 1112 | 409 | html | https://www.tarotnow.xyz/vi/profile |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile | GET | 200 | 1070 | 315 | html | https://www.tarotnow.xyz/vi/profile |
| logged-out | desktop | auth-public | /vi | GET | 200 | 1053 | 371 | html | https://www.tarotnow.xyz/vi |
| logged-out | desktop | auth-public | /vi | GET | 200 | 1028 | 547 | static | https://www.tarotnow.xyz/_next/static/chunks/0kxzca98t52gs.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 1018 | 548 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-out | mobile | auth-public | /vi | GET | 200 | 1001 | 583 | html | https://www.tarotnow.xyz/vi |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | GET | 200 | 996 | 316 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 968 | 506 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1920&q=75 |
| logged-in-reader | desktop | reader-chat | /vi/reader/apply | GET | 200 | 872 | 335 | html | https://www.tarotnow.xyz/vi/reader/apply |
| logged-in-admin | desktop | reading | /vi/reading | GET | 200 | 864 | 379 | html | https://www.tarotnow.xyz/vi/reading |
| logged-in-reader | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | GET | 200 | 861 | 331 | html | https://www.tarotnow.xyz/vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | GET | 200 | 845 | 324 | html | https://www.tarotnow.xyz/vi/wallet/deposit |
| logged-out | desktop | auth-public | /vi/login | GET | 200 | 826 | 342 | static | https://www.tarotnow.xyz/_next/static/chunks/0kxzca98t52gs.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 817 | 315 | html | https://www.tarotnow.xyz/vi/notifications |
| logged-in-admin | desktop | admin | /vi/admin/deposits | GET | 200 | 816 | 345 | html | https://www.tarotnow.xyz/vi/admin/deposits |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/inventory | GET | 200 | 813 | 317 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-out | desktop | auth-public | /vi/login | GET | 200 | 812 | 323 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-out | desktop | auth-public | /vi/login | GET | 200 | 811 | 403 | html | https://www.tarotnow.xyz/vi/login |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 809 | 318 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | GET | 200 | 804 | 300 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | GET | 200 | 802 | 332 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-admin | desktop | reader-chat | /vi/readers | GET | 200 | 802 | 320 | html | https://www.tarotnow.xyz/vi/readers |
| logged-in-reader | desktop | reader-chat | /vi/readers | GET | 200 | 802 | 315 | html | https://www.tarotnow.xyz/vi/readers |

## Medium Slow Requests (400-800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| logged-in-reader | mobile | reading | /vi/reading | GET | 200 | 800 | 317 | html | https://www.tarotnow.xyz/vi/reading |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | GET | 200 | 799 | 317 | html | https://www.tarotnow.xyz/vi/notifications |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | GET | 200 | 799 | 324 | html | https://www.tarotnow.xyz/vi/wallet |
| logged-out | desktop | auth-public | /vi | GET | 200 | 798 | 553 | static | https://www.tarotnow.xyz/_next/static/chunks/0u95sv2h1cu3o.js |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | GET | 200 | 798 | 311 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 798 | 298 | html | https://www.tarotnow.xyz/vi/readers/97410d49-ab32-469d-b421-f57321b262a2 |
| logged-in-admin | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 797 | 325 | html | https://www.tarotnow.xyz/vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | GET | 200 | 797 | 319 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | GET | 200 | 796 | 310 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 795 | 320 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-admin | desktop | admin | /vi/admin | GET | 200 | 795 | 550 | static | https://www.tarotnow.xyz/_next/static/chunks/16o0.rxdplbck.js |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | GET | 200 | 795 | 310 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-in-admin | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 795 | 335 | html | https://www.tarotnow.xyz/vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 |
| logged-out | desktop | auth-public | /vi/verify-email | GET | 200 | 792 | 392 | html | https://www.tarotnow.xyz/vi/verify-email |
| logged-in-reader | mobile | reader-chat | /vi/readers | GET | 200 | 789 | 310 | html | https://www.tarotnow.xyz/vi/readers |
| logged-in-admin | desktop | reading | /vi/reading/session/60daf472-49db-4823-9fd9-2bdc883e1923 | GET | 200 | 787 | 353 | html | https://www.tarotnow.xyz/vi/reading/session/60daf472-49db-4823-9fd9-2bdc883e1923 |
| logged-in-admin | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 785 | 322 | html | https://www.tarotnow.xyz/vi/readers/97410d49-ab32-469d-b421-f57321b262a2 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | GET | 200 | 784 | 302 | html | https://www.tarotnow.xyz/vi/profile |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/gamification | GET | 200 | 783 | 327 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-in-admin | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 783 | 318 | html | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 782 | 338 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-admin | desktop | admin | /vi/admin | GET | 200 | 782 | 543 | static | https://www.tarotnow.xyz/_next/static/chunks/0jsr-cskz27s0.js |
| logged-in-admin | desktop | admin | /vi/admin/promotions | GET | 200 | 782 | 326 | html | https://www.tarotnow.xyz/vi/admin/promotions |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 781 | 341 | html | https://www.tarotnow.xyz/vi/profile/reader |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 781 | 311 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 780 | 304 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-reader | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 780 | 355 | html | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| logged-in-admin | desktop | admin | /vi/admin/users | GET | 200 | 779 | 345 | html | https://www.tarotnow.xyz/vi/admin/users |
| logged-in-reader | desktop | reading | /vi/reading | GET | 200 | 779 | 311 | html | https://www.tarotnow.xyz/vi/reading |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 776 | 306 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | GET | 200 | 775 | 326 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/gamification | GET | 200 | 775 | 320 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-in-admin | mobile | reading | /vi/reading | GET | 200 | 774 | 309 | html | https://www.tarotnow.xyz/vi/reading |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | GET | 200 | 767 | 341 | html | https://www.tarotnow.xyz/vi |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | GET | 200 | 767 | 310 | html | https://www.tarotnow.xyz/vi/profile |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 766 | 324 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-admin | desktop | admin | /vi/admin/system-configs | GET | 200 | 766 | 366 | html | https://www.tarotnow.xyz/vi/admin/system-configs |
| logged-in-admin | mobile | reading | /vi/reading/history | GET | 200 | 765 | 317 | html | https://www.tarotnow.xyz/vi/reading/history |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 764 | 378 | html | https://www.tarotnow.xyz/vi/profile/mfa |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | GET | 200 | 764 | 306 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-in-admin | mobile | reader-chat | /vi/readers | GET | 200 | 764 | 304 | html | https://www.tarotnow.xyz/vi/readers |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 763 | 374 | html | https://www.tarotnow.xyz/vi/profile/reader |
| logged-in-admin | desktop | admin | /vi/admin/readings | GET | 200 | 761 | 320 | html | https://www.tarotnow.xyz/vi/admin/readings |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/notifications | GET | 200 | 760 | 317 | html | https://www.tarotnow.xyz/vi/notifications |
| logged-in-admin | desktop | admin | /vi/admin/disputes | GET | 200 | 760 | 351 | html | https://www.tarotnow.xyz/vi/admin/disputes |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 760 | 292 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1920&q=75 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | GET | 200 | 757 | 339 | html | https://www.tarotnow.xyz/vi/wallet |
| logged-out | desktop | auth-public | /vi/reset-password | GET | 200 | 756 | 365 | html | https://www.tarotnow.xyz/vi/reset-password |
| logged-in-admin | desktop | admin | /vi/admin/reader-requests | GET | 200 | 754 | 326 | html | https://www.tarotnow.xyz/vi/admin/reader-requests |
| logged-in-reader | desktop | reading | /vi/reading/history | GET | 200 | 746 | 314 | html | https://www.tarotnow.xyz/vi/reading/history |
| logged-in-admin | mobile | admin | /vi/admin/deposits | GET | 200 | 746 | 343 | html | https://www.tarotnow.xyz/vi/admin/deposits |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 743 | 317 | html | https://www.tarotnow.xyz/vi/notifications |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 741 | 305 | html | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| logged-in-admin | desktop | admin | /vi/admin | GET | 200 | 740 | 338 | html | https://www.tarotnow.xyz/vi/admin |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 740 | 310 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | GET | 200 | 739 | 327 | html | https://www.tarotnow.xyz/vi/collection |
| logged-in-admin | mobile | admin | /vi/admin/gamification | GET | 200 | 737 | 318 | html | https://www.tarotnow.xyz/vi/admin/gamification |
| logged-in-reader | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 736 | 304 | html | https://www.tarotnow.xyz/vi/readers/97410d49-ab32-469d-b421-f57321b262a2 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 736 | 311 | html | https://www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | GET | 200 | 734 | 534 | static | https://www.tarotnow.xyz/_next/static/chunks/0kxzca98t52gs.js |
| logged-in-admin | desktop | admin | /vi/admin/withdrawals | GET | 200 | 730 | 364 | html | https://www.tarotnow.xyz/vi/admin/withdrawals |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 730 | 375 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-reader | desktop | auth-public | /vi/legal/tos | GET | 200 | 728 | 317 | html | https://www.tarotnow.xyz/vi/legal/tos |
| logged-in-reader | mobile | reading | /vi/reading/session/b5b51196-e110-4046-9a8d-05e196b6aa1f | GET | 200 | 725 | 310 | html | https://www.tarotnow.xyz/vi/reading/session/b5b51196-e110-4046-9a8d-05e196b6aa1f |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 719 | 319 | html | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet | GET | 200 | 717 | 347 | html | https://www.tarotnow.xyz/vi/wallet |
| logged-out | mobile | auth-public | /vi/legal/ai-disclaimer | GET | 200 | 717 | 347 | html | https://www.tarotnow.xyz/vi/legal/ai-disclaimer |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 717 | 325 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-admin | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 715 | 329 | html | https://www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| logged-in-reader | desktop | auth-public | /vi/legal/ai-disclaimer | GET | 200 | 715 | 341 | html | https://www.tarotnow.xyz/vi/legal/ai-disclaimer |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 714 | 330 | html | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 714 | 342 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-admin | desktop | reading | /vi/reading/session/f335007c-d308-482f-b651-b34e2b32a50b | GET | 200 | 710 | 321 | html | https://www.tarotnow.xyz/vi/reading/session/f335007c-d308-482f-b651-b34e2b32a50b |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | GET | 200 | 710 | 319 | html | https://www.tarotnow.xyz/vi/collection |
| logged-in-admin | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | GET | 200 | 709 | 316 | html | https://www.tarotnow.xyz/vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | GET | 200 | 706 | 433 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-reader | desktop | reading | /vi/reading/session/e9280543-e216-42d0-bcb7-cc10b2e8fad7 | GET | 200 | 706 | 303 | html | https://www.tarotnow.xyz/vi/reading/session/e9280543-e216-42d0-bcb7-cc10b2e8fad7 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 706 | 329 | html | https://www.tarotnow.xyz/vi/profile/mfa |
| logged-in-admin | desktop | reader-chat | /vi/chat | GET | 200 | 705 | 329 | html | https://www.tarotnow.xyz/vi/chat |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | GET | 200 | 705 | 314 | html | https://www.tarotnow.xyz/vi/wallet/deposit |

## Pending Requests
| Scenario | Viewport | Route | URL |
| --- | --- | --- | --- |
| - | - | - | - |

## Coverage Notes
| Scenario | Viewport | Note |
| --- | --- | --- |
| logged-out | desktop | origin-discovery:/sitemap.xml:routes-22 |
| logged-out | desktop | origin-discovery:/robots.txt:routes-22 |
| logged-out | desktop | dynamic-routes: skipped for logged-out scenario. |
| logged-out | desktop | scenario-filter:logged-out-protected-routes-skipped=30 |
| logged-in-admin | desktop | origin-discovery:/sitemap.xml:routes-22 |
| logged-in-admin | desktop | origin-discovery:/robots.txt:routes-22 |
| logged-in-admin | desktop | reading.init.daily_1: blocked (400). |
| logged-in-admin | desktop | reading.init.spread_3: created 60daf472-49db-4823-9fd9-2bdc883e1923. |
| logged-in-admin | desktop | reading.init.spread_5: created 9d89d38d-8ea2-4420-b477-d1cd83d81a7b. |
| logged-in-admin | desktop | reading.init.spread_10: created f335007c-d308-482f-b651-b34e2b32a50b. |
| logged-in-admin | desktop | reading-history-detail: coverage-blocked (no history id found). |
| logged-in-admin | desktop | chat-room-detail: coverage-blocked (no conversation id found). |
| logged-in-admin | desktop | reader-detail:ui-discovery-empty |
| logged-in-admin | desktop | chat-room-detail:ui-discovery-empty |
| logged-in-admin | desktop | reading-history-detail:ui-discovery-empty |
| logged-in-admin | desktop | community-posts:api-discovery-1 |
| logged-in-admin | desktop | community-post-detail:69db54fc297f66f734421a3c:stale-page-404 |
| logged-in-admin | desktop | scenario-filter:admin-auth-entry-routes-skipped=5 |
| logged-in-reader | desktop | origin-discovery:/sitemap.xml:routes-22 |
| logged-in-reader | desktop | origin-discovery:/robots.txt:routes-22 |
| logged-in-reader | desktop | reading.init.daily_1: blocked (400). |
| logged-in-reader | desktop | reading.init.spread_3: created efd7e84f-9903-48ac-804d-6b36bb38b044. |
| logged-in-reader | desktop | reading.init.spread_5: created e9280543-e216-42d0-bcb7-cc10b2e8fad7. |
| logged-in-reader | desktop | reading.init.spread_10: created ac1b0435-eb90-4f53-a70f-15c12a1bb5ba. |
| logged-in-reader | desktop | reading-history-detail: coverage-blocked (no history id found). |
| logged-in-reader | desktop | chat-room-detail: coverage-blocked (no conversation id found). |
| logged-in-reader | desktop | reader-detail:ui-discovery-empty |
| logged-in-reader | desktop | chat-room-detail:ui-discovery-empty |
| logged-in-reader | desktop | reading-history-detail:ui-discovery-empty |
| logged-in-reader | desktop | community-posts:api-discovery-1 |
| logged-in-reader | desktop | community-post-detail:69db54fc297f66f734421a3c:stale-page-404 |
| logged-in-reader | desktop | scenario-filter:reader-auth-entry-admin-routes-skipped=15 |
| logged-out | mobile | origin-discovery:/sitemap.xml:routes-22 |
| logged-out | mobile | origin-discovery:/robots.txt:routes-22 |
| logged-out | mobile | dynamic-routes: skipped for logged-out scenario. |
| logged-out | mobile | scenario-filter:logged-out-protected-routes-skipped=30 |
| logged-in-admin | mobile | origin-discovery:/sitemap.xml:routes-22 |
| logged-in-admin | mobile | origin-discovery:/robots.txt:routes-22 |
| logged-in-admin | mobile | reading.init.daily_1: blocked (400). |
| logged-in-admin | mobile | reading.init.spread_3: created 491fbd8e-0ff1-4d60-9edc-786f508c66f5. |
| logged-in-admin | mobile | reading.init.spread_5: created 6134d2b7-29af-4706-bdca-3f807b84a4a9. |
| logged-in-admin | mobile | reading.init.spread_10: created 9bbdb4cc-e46e-4164-9a06-a8fdd18887cd. |
| logged-in-admin | mobile | reading-history-detail: coverage-blocked (no history id found). |
| logged-in-admin | mobile | chat-room-detail: coverage-blocked (no conversation id found). |
| logged-in-admin | mobile | reader-detail:ui-discovery-empty |
| logged-in-admin | mobile | chat-room-detail:ui-discovery-empty |
| logged-in-admin | mobile | reading-history-detail:ui-discovery-empty |
| logged-in-admin | mobile | community-posts:api-discovery-1 |
| logged-in-admin | mobile | community-post-detail:69db54fc297f66f734421a3c:stale-page-404 |
| logged-in-admin | mobile | scenario-filter:admin-auth-entry-routes-skipped=5 |
| logged-in-reader | mobile | origin-discovery:/sitemap.xml:routes-22 |
| logged-in-reader | mobile | origin-discovery:/robots.txt:routes-22 |
| logged-in-reader | mobile | reading.init.daily_1: blocked (400). |
| logged-in-reader | mobile | reading.init.spread_3: created b5b51196-e110-4046-9a8d-05e196b6aa1f. |
| logged-in-reader | mobile | reading.init.spread_5: created 0cbbf7e2-7126-4cc2-ba5b-2e92e340041b. |
| logged-in-reader | mobile | reading.init.spread_10: created 1796af3f-42f6-4466-9c2e-8f5e9dc72105. |
| logged-in-reader | mobile | reading-history-detail: coverage-blocked (no history id found). |
| logged-in-reader | mobile | chat-room-detail: coverage-blocked (no conversation id found). |
| logged-in-reader | mobile | reader-detail:ui-discovery-empty |
| logged-in-reader | mobile | chat-room-detail:ui-discovery-empty |
| logged-in-reader | mobile | reading-history-detail:ui-discovery-empty |
| logged-in-reader | mobile | community-posts:api-discovery-1 |
| logged-in-reader | mobile | community-post-detail:69db54fc297f66f734421a3c:stale-page-404 |
| logged-in-reader | mobile | scenario-filter:reader-auth-entry-admin-routes-skipped=15 |

## Login Bootstrap Notes
### logged-in-admin / desktop
- Attempt 1: login bootstrap succeeded.

### logged-in-reader / desktop
- Attempt 1: login bootstrap succeeded.

### logged-in-admin / mobile
- Attempt 1: login bootstrap succeeded.

### logged-in-reader / mobile
- Attempt 1: login bootstrap succeeded.

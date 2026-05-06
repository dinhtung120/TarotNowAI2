import { defineConfig, globalIgnores } from "eslint/config";
import nextVitals from "eslint-config-next/core-web-vitals";
import nextTs from "eslint-config-next/typescript";

const eslintConfig = defineConfig([
  ...nextVitals,
  ...nextTs,
  {
    files: ["src/**/*.{ts,tsx}"],
    ignores: [
      "src/**/*.test.{ts,tsx}",
      "src/shared/infrastructure/http/apiUrl.ts",
      "src/shared/infrastructure/http/clientJsonRequest.ts",
    ],
    rules: {
      "no-restricted-imports": [
        "error",
        {
          paths: [
            {
              name: "@/shared/infrastructure/http/apiUrl",
              importNames: ["getPublicApiBaseUrl"],
              message:
                "Use local BFF routes or shared clientJsonRequest helpers instead of reading NEXT_PUBLIC_API_URL directly.",
            },
          ],
        },
      ],
      "no-restricted-syntax": [
        "error",
        {
          selector:
            "MemberExpression[object.object.name='process'][object.property.name='env'][property.name='NEXT_PUBLIC_API_URL']",
          message:
            "Read NEXT_PUBLIC_API_URL only inside shared/infrastructure/http/apiUrl.ts.",
        },
        {
          selector: "CallExpression[callee.name='useAuthStore'][arguments.length=0]",
          message:
            "useAuthStore must use a selector to avoid unnecessary re-renders.",
        },
      ],
    },
  },
  {
    files: ["src/features/gacha/shared/**/*.{ts,tsx}"],
    rules: {
      "no-param-reassign": ["error", { props: true }],
    },
  },
  
  globalIgnores([
    
    ".next/**",
    "coverage/**",
    "out/**",
    "build/**",
    "public/vendor/**",
    "next-env.d.ts",
  ]),
]);

export default eslintConfig;

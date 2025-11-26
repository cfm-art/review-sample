# review-sample

## TODO管理アプリケーション

コードレビューの勉強に有用な、シンプルなTODO管理APIアプリケーションです。

### 技術スタック

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- SQLite

### 機能

- TODO項目の作成
- TODO項目の一覧取得
- TODO項目の詳細取得
- TODO項目の更新
- TODO項目の削除
- 完了状態の管理（完了日時の自動記録）

### プロジェクト構成

```
TodoApi/
├── Models/
│   └── TodoItem.cs          # TODOデータモデル
├── Data/
│   └── TodoContext.cs       # データベースコンテキスト
├── Program.cs               # アプリケーションエントリポイント
├── appsettings.json         # 設定ファイル
└── todos.db                 # SQLiteデータベース（実行時に作成）
```

### セットアップ

1. リポジトリをクローン
```bash
git clone <repository-url>
cd review-sample
```

2. プロジェクトディレクトリに移動
```bash
cd TodoApi
```

3. アプリケーションの実行
```bash
dotnet run
```

アプリケーションは `http://localhost:5175` で起動します。

### API エンドポイント

#### 全てのTODO項目を取得
```bash
GET /api/todos
```

#### 特定のTODO項目を取得
```bash
GET /api/todos/{id}
```

#### TODO項目を作成
```bash
POST /api/todos
Content-Type: application/json

{
  "title": "タイトル",
  "description": "説明（任意）",
  "isCompleted": false
}
```

#### TODO項目を更新
```bash
PUT /api/todos/{id}
Content-Type: application/json

{
  "title": "更新されたタイトル",
  "description": "更新された説明",
  "isCompleted": true
}
```

#### TODO項目を削除
```bash
DELETE /api/todos/{id}
```

### 使用例

```bash
# TODO項目を作成
curl -X POST http://localhost:5175/api/todos \
  -H "Content-Type: application/json" \
  -d '{"title":"買い物","description":"牛乳、卵、パン","isCompleted":false}'

# 全てのTODO項目を取得
curl http://localhost:5175/api/todos

# TODO項目を完了にする
curl -X PUT http://localhost:5175/api/todos/1 \
  -H "Content-Type: application/json" \
  -d '{"title":"買い物","description":"牛乳、卵、パン","isCompleted":true}'

# TODO項目を削除
curl -X DELETE http://localhost:5175/api/todos/1
```

### データモデル

```csharp
public class TodoItem
{
    public int Id { get; set; }
    public string Title { get; set; }           // 必須、最大200文字
    public string? Description { get; set; }    // 任意、最大1000文字
    public bool IsCompleted { get; set; }       // 完了状態
    public DateTime CreatedAt { get; set; }     // 作成日時（自動設定）
    public DateTime? CompletedAt { get; set; }  // 完了日時（完了時に自動設定）
}
```

### ビルド

```bash
cd TodoApi
dotnet build
```

### 開発環境

- .NET SDK 10.0以上が必要です

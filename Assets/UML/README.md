## 概要

UML記述による描画

## 使用Unityバージョン

- Unity **6000.3.10f1**
- plantumlclassdiagramgenerator **1.4.0** 
---

## セットアップ方法

### 0. `plantumlclassdiagramgenerator`をインストール
```bash
dotnet tool install --global PlantUmlClassDiagramGenerator
```

### 1. `.puml`の生成
`team-farm-proto`をカレントディレクトリに

```bash
puml-gen .\Assets\Scripts .\Assets\UML -dir -createAssociation -allInOne
```

### 2. 画像化

#### 2.1. `.png`
```bash
java -jar "C:\PlantUML\plantuml-1.2026.6.jar" ".\Assets\UML\include.puml"
```
 #### 2.2. `.SVG`
```bash
java -jar "C:\PlantUML\plantuml-1.2026.6.jar" -tsvg ".\Assets\UML\include.puml"
```

---

## トラブルシューティング
`.puml`生成の際にリストが正しく結びつかないことがある。  

1. 配列を使用しない。
配列だとUMLの自動生成と整合性がない。

``` C#
using System.Collections.Generic;
```
を宣言。そして、配列をList<>に変更
``` C#
[] -> List<>
```

2. 正規表現による置換 
  
検索: `(-->|o->)\s+"(.*?)<(.*?)>"\s+".*?"`  
  
置換: `o-> "$2" $3`  

<template>
  <div class="goods-page">
    <el-card>
      <template #header>
        <div class="card-header">
          <span>商品管理</span>
          <el-button type="primary" @click="handleAdd">新增商品</el-button>
        </div>
      </template>

      <el-form :inline="true" :model="searchForm" class="search-form">
        <el-form-item label="商品名称">
          <el-input v-model="searchForm.keyword" placeholder="请输入商品名称" clearable />
        </el-form-item>
        <el-form-item label="分类">
          <el-select v-model="searchForm.categoryId" placeholder="请选择分类" clearable>
            <el-option
              v-for="item in categories"
              :key="item.id"
              :label="item.name"
              :value="item.id"
            />
          </el-select>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" @click="handleSearch">搜索</el-button>
          <el-button @click="handleReset">重置</el-button>
        </el-form-item>
      </el-form>

      <el-table :data="goodsList" v-loading="loading" border>
        <el-table-column type="index" width="50" />
        <el-table-column label="商品图片" width="100">
          <template #default="{ row }">
            <el-image
              :src="row.mainPicture"
              :preview-src-list="[row.mainPicture]"
              style="width: 60px; height: 60px"
              fit="cover"
            />
          </template>
        </el-table-column>
        <el-table-column prop="name" label="商品名称" min-width="200" show-overflow-tooltip />
        <el-table-column prop="price" label="价格" width="120">
          <template #default="{ row }">
            <span style="color: #f56c6c">¥{{ row.price }}</span>
          </template>
        </el-table-column>
        <el-table-column prop="inventory" label="库存" width="100" />
        <el-table-column prop="salesCount" label="销量" width="100" />
        <el-table-column prop="categoryName" label="分类" width="120" />
        <el-table-column prop="status" label="状态" width="100">
          <template #default="{ row }">
            <el-tag :type="row.status === 1 ? 'success' : 'info'">
              {{ row.status === 1 ? '上架' : '下架' }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column label="操作" width="200" fixed="right">
          <template #default="{ row }">
            <el-button type="primary" link @click="handleEdit(row)">编辑</el-button>
            <el-button type="danger" link @click="handleDelete(row)">删除</el-button>
          </template>
        </el-table-column>
      </el-table>

      <div class="pagination">
        <el-pagination
          v-model:current-page="page"
          v-model:page-size="pageSize"
          :total="total"
          :page-sizes="[10, 20, 50, 100]"
          layout="total, sizes, prev, pager, next"
          @size-change="handleSizeChange"
          @current-change="handleCurrentChange"
        />
      </div>
    </el-card>

    <el-dialog
      v-model="dialogVisible"
      :title="dialogTitle"
      width="700px"
      destroy-on-close
    >
      <el-form
        ref="formRef"
        :model="form"
        :rules="rules"
        label-width="100px"
      >
        <el-form-item label="商品名称" prop="name">
          <el-input v-model="form.name" placeholder="请输入商品名称" />
        </el-form-item>
        <el-form-item label="商品描述" prop="desc">
          <el-input
            v-model="form.desc"
            type="textarea"
            :rows="3"
            placeholder="请输入商品描述"
          />
        </el-form-item>
        <el-form-item label="商品价格" prop="price">
          <el-input-number v-model="form.price" :min="0" :precision="2" />
        </el-form-item>
        <el-form-item label="原价" prop="oldPrice">
          <el-input-number v-model="form.oldPrice" :min="0" :precision="2" />
        </el-form-item>
        <el-form-item label="库存" prop="inventory">
          <el-input-number v-model="form.inventory" :min="0" :precision="0" />
        </el-form-item>
        <el-form-item label="分类" prop="categoryId">
          <el-select v-model="form.categoryId" placeholder="请选择分类">
            <el-option
              v-for="item in categories"
              :key="item.id"
              :label="item.name"
              :value="item.id"
            />
          </el-select>
        </el-form-item>
        <el-form-item label="商品图片" prop="mainPicture">
          <el-input v-model="form.mainPicture" placeholder="请输入图片URL" />
        </el-form-item>
        <el-form-item label="状态" prop="status">
          <el-radio-group v-model="form.status">
            <el-radio :label="1">上架</el-radio>
            <el-radio :label="0">下架</el-radio>
          </el-radio-group>
        </el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="dialogVisible = false">取消</el-button>
        <el-button type="primary" :loading="submitLoading" @click="handleSubmit">确定</el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import { getGoodsList, createGoods, updateGoods, deleteGoods, getCategories } from '@/api/goods'
import type { Goods, Category } from '@/types'

const loading = ref(false)
const goodsList = ref<Goods[]>([])
const categories = ref<Category[]>([])
const page = ref(1)
const pageSize = ref(10)
const total = ref(0)

const searchForm = reactive({
  keyword: '',
  categoryId: ''
})

const dialogVisible = ref(false)
const dialogTitle = ref('新增商品')
const submitLoading = ref(false)
const formRef = ref()
const currentId = ref('')

const form = reactive({
  name: '',
  desc: '',
  price: 0,
  oldPrice: 0,
  inventory: 0,
  categoryId: '',
  mainPicture: '',
  status: 1
})

const rules = {
  name: [{ required: true, message: '请输入商品名称', trigger: 'blur' }],
  price: [{ required: true, message: '请输入商品价格', trigger: 'blur' }],
  categoryId: [{ required: true, message: '请选择分类', trigger: 'change' }]
}

const fetchGoodsList = async () => {
  loading.value = true
  try {
    const result = await getGoodsList({
      page: page.value,
      pageSize: pageSize.value,
      keyword: searchForm.keyword,
      categoryId: searchForm.categoryId
    })
    goodsList.value = result.list
    total.value = result.total
  } catch (error) {
    console.error(error)
  } finally {
    loading.value = false
  }
}

const fetchCategories = async () => {
  try {
    const result = await getCategories()
    categories.value = result
  } catch (error) {
    console.error(error)
  }
}

const handleSearch = () => {
  page.value = 1
  fetchGoodsList()
}

const handleReset = () => {
  searchForm.keyword = ''
  searchForm.categoryId = ''
  page.value = 1
  fetchGoodsList()
}

const handleAdd = () => {
  dialogTitle.value = '新增商品'
  currentId.value = ''
  form.name = ''
  form.desc = ''
  form.price = 0
  form.oldPrice = 0
  form.inventory = 0
  form.categoryId = ''
  form.mainPicture = ''
  form.status = 1
  dialogVisible.value = true
}

const handleEdit = (row: Goods) => {
  dialogTitle.value = '编辑商品'
  currentId.value = row.id
  form.name = row.name
  form.desc = row.desc || ''
  form.price = row.price
  form.oldPrice = row.oldPrice
  form.inventory = row.inventory
  form.categoryId = row.categoryId || ''
  form.mainPicture = row.mainPicture || ''
  form.status = row.status
  dialogVisible.value = true
}

const handleDelete = async (row: Goods) => {
  try {
    await ElMessageBox.confirm('确定要删除该商品吗？', '提示', {
      type: 'warning'
    })
    await deleteGoods(row.id)
    ElMessage.success('删除成功')
    fetchGoodsList()
  } catch (error) {
    console.error(error)
  }
}

const handleSubmit = async () => {
  const valid = await formRef.value?.validate()
  if (!valid) return

  submitLoading.value = true
  try {
    if (currentId.value) {
      await updateGoods(currentId.value, form)
      ElMessage.success('修改成功')
    } else {
      await createGoods(form)
      ElMessage.success('添加成功')
    }
    dialogVisible.value = false
    fetchGoodsList()
  } catch (error) {
    console.error(error)
  } finally {
    submitLoading.value = false
  }
}

const handleSizeChange = (val: number) => {
  pageSize.value = val
  fetchGoodsList()
}

const handleCurrentChange = (val: number) => {
  page.value = val
  fetchGoodsList()
}

onMounted(() => {
  fetchGoodsList()
  fetchCategories()
})
</script>

<style scoped lang="scss">
.goods-page {
  .card-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
  }

  .search-form {
    margin-bottom: 20px;
  }

  .pagination {
    margin-top: 20px;
    display: flex;
    justify-content: flex-end;
  }
}
</style>

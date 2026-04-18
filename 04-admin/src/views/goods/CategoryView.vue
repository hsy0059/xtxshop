<template>
  <div class="category-page">
    <el-card>
      <template #header>
        <div class="card-header">
          <span>分类管理</span>
          <el-button type="primary" @click="handleAdd">新增分类</el-button>
        </div>
      </template>

      <el-table :data="categoryList" v-loading="loading" row-key="id" border>
        <el-table-column type="index" width="50" />
        <el-table-column label="分类图标" width="100">
          <template #default="{ row }">
            <el-image
              :src="row.icon || row.picture"
              style="width: 50px; height: 50px"
              fit="cover"
            />
          </template>
        </el-table-column>
        <el-table-column prop="name" label="分类名称" min-width="200" />
        <el-table-column prop="level" label="层级" width="100">
          <template #default="{ row }">
            <el-tag>{{ row.level === 1 ? '一级' : '二级' }}</el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="sort" label="排序" width="100" />
        <el-table-column label="操作" width="200" fixed="right">
          <template #default="{ row }">
            <el-button type="primary" link @click="handleEdit(row)">编辑</el-button>
            <el-button type="danger" link @click="handleDelete(row)">删除</el-button>
          </template>
        </el-table-column>
      </el-table>
    </el-card>

    <el-dialog
      v-model="dialogVisible"
      :title="dialogTitle"
      width="500px"
      destroy-on-close
    >
      <el-form
        ref="formRef"
        :model="form"
        :rules="rules"
        label-width="100px"
      >
        <el-form-item label="分类名称" prop="name">
          <el-input v-model="form.name" placeholder="请输入分类名称" />
        </el-form-item>
        <el-form-item label="上级分类" prop="parentId">
          <el-select v-model="form.parentId" placeholder="请选择上级分类" clearable>
            <el-option
              v-for="item in parentCategories"
              :key="item.id"
              :label="item.name"
              :value="item.id"
            />
          </el-select>
        </el-form-item>
        <el-form-item label="分类图标" prop="icon">
          <el-input v-model="form.icon" placeholder="请输入图标URL" />
        </el-form-item>
        <el-form-item label="分类图片" prop="picture">
          <el-input v-model="form.picture" placeholder="请输入图片URL" />
        </el-form-item>
        <el-form-item label="排序" prop="sort">
          <el-input-number v-model="form.sort" :min="0" />
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
import { ref, reactive, onMounted, computed } from 'vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import { getCategories, createCategory, updateCategory, deleteCategory } from '@/api/goods'
import type { Category } from '@/types'

const loading = ref(false)
const categoryList = ref<Category[]>([])
const dialogVisible = ref(false)
const dialogTitle = ref('新增分类')
const submitLoading = ref(false)
const formRef = ref()
const currentId = ref('')

const form = reactive({
  name: '',
  parentId: '',
  icon: '',
  picture: '',
  sort: 0
})

const rules = {
  name: [{ required: true, message: '请输入分类名称', trigger: 'blur' }]
}

const parentCategories = computed(() => {
  return categoryList.value.filter(item => item.level === 1)
})

const flattenCategories = (categories: Category[]): Category[] => {
  const result: Category[] = []
  categories.forEach(item => {
    result.push(item)
    if (item.children && item.children.length > 0) {
      result.push(...flattenCategories(item.children))
    }
  })
  return result
}

const fetchCategories = async () => {
  loading.value = true
  try {
    const result = await getCategories()
    categoryList.value = flattenCategories(result)
  } catch (error) {
    console.error(error)
  } finally {
    loading.value = false
  }
}

const handleAdd = () => {
  dialogTitle.value = '新增分类'
  currentId.value = ''
  form.name = ''
  form.parentId = ''
  form.icon = ''
  form.picture = ''
  form.sort = 0
  dialogVisible.value = true
}

const handleEdit = (row: Category) => {
  dialogTitle.value = '编辑分类'
  currentId.value = row.id
  form.name = row.name
  form.parentId = row.parentId || ''
  form.icon = row.icon || ''
  form.picture = row.picture || ''
  form.sort = row.sort
  dialogVisible.value = true
}

const handleDelete = async (row: Category) => {
  try {
    await ElMessageBox.confirm('确定要删除该分类吗？', '提示', {
      type: 'warning'
    })
    await deleteCategory(row.id)
    ElMessage.success('删除成功')
    fetchCategories()
  } catch (error) {
    console.error(error)
  }
}

const handleSubmit = async () => {
  const valid = await formRef.value?.validate()
  if (!valid) return

  submitLoading.value = true
  try {
    const data = {
      ...form,
      level: form.parentId ? 2 : 1
    }
    if (currentId.value) {
      await updateCategory(currentId.value, data)
      ElMessage.success('修改成功')
    } else {
      await createCategory(data)
      ElMessage.success('添加成功')
    }
    dialogVisible.value = false
    fetchCategories()
  } catch (error) {
    console.error(error)
  } finally {
    submitLoading.value = false
  }
}

onMounted(() => {
  fetchCategories()
})
</script>

<style scoped lang="scss">
.category-page {
  .card-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
  }
}
</style>
